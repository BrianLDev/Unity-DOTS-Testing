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
        // make copy of data, modify it, then copy back
        HealthData healthA = healthGroup[entityA];
        HealthData healthB = healthGroup[entityB];
        healthA.health -= healthB.damageCaused;
        healthB.health -= healthA.damageCaused;
        if (healthA.health <= 0)
          healthA.isDead = true;
        if (healthB.health <= 0)
          healthB.isDead = true;
        healthGroup[entityA] = healthA;
        healthGroup[entityB] = healthB;
      }
    }
  }
}