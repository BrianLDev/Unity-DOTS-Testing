using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Physics;
using Unity.Physics.Systems;

public partial class DamageOnCollisionSystem : SystemBase
{
  private BuildPhysicsWorld buildPhysicsWorld;
  private StepPhysicsWorld stepPhysicsWorld;

  protected override void OnCreate()
  {
    base.OnCreate();
    buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
    stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
  }

  protected override void OnUpdate()
  {
    // reduce invulnerable timers
    float deltaTime = Time.DeltaTime;
    Entities.ForEach((ref HealthData healthData) => 
    {
      if (healthData.invulnerableTimer > 0)
        healthData.invulnerableTimer -= deltaTime; 
    }).ScheduleParallel();

    // Damage On Collision Job
    Dependency = new DamageOnCollisionSystemJob
    {
      healthGroup = GetComponentDataFromEntity<HealthData>()
    }.Schedule(stepPhysicsWorld.Simulation, Dependency);
    
    Dependency.Complete();
  }

  [BurstCompile]
  public struct DamageOnCollisionSystemJob : ICollisionEventsJob
  {
    public ComponentDataFromEntity<HealthData> healthGroup;

    public void Execute(CollisionEvent collisionEvent)
    {
      Entity entityA = collisionEvent.EntityA;
      Entity entityB = collisionEvent.EntityB;

      if (healthGroup.HasComponent(entityA) && healthGroup.HasComponent(entityB)) 
      {
        if (healthGroup[entityA].invulnerableTimer > 0 || healthGroup[entityB].invulnerableTimer > 0)
          return;
        else
        {
          // make a copy of data, modify it, then copy back
          HealthData healthA = healthGroup[entityA];
          HealthData healthB = healthGroup[entityB];
          healthA.health -= healthB.damageCaused;
          healthB.health -= healthA.damageCaused;
          if (healthA.health <= 0)
            healthA.isDead = true;
          else
            healthA.invulnerableTimer = 0.5f;
          if (healthB.health <= 0)
            healthB.isDead = true;
          else
            healthB.invulnerableTimer = 0.5f;
          healthGroup[entityA] = healthA;
          healthGroup[entityB] = healthB;
        }
      }
    }
  }
}