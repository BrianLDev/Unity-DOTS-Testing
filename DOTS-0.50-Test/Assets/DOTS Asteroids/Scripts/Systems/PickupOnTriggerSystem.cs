using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;

public partial class PickupOnTriggerSystem : SystemBase
{
  private StepPhysicsWorld stepPhysicsWorld;    // running simulation
  private EndSimulationEntityCommandBufferSystem commandBufferSystem;

  protected override void OnCreate()
  {
    base.OnCreate();
    stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
  }

  protected override void OnStartRunning()
  {
    this.RegisterPhysicsRuntimeSystemReadOnly();
  }

  protected override void OnUpdate()
  {
    var entityCommandBuffer = commandBufferSystem.CreateCommandBuffer();
    Dependency = new PickupOnTriggerSystemJob
    {
      allPickups = GetComponentDataFromEntity<PickupTag>(true),
      allPlayers = GetComponentDataFromEntity<PlayerTag>(),
      ecb = entityCommandBuffer
    }.Schedule(stepPhysicsWorld.Simulation, Dependency);
    
    Dependency.Complete();
    commandBufferSystem.AddJobHandleForProducer(this.Dependency);
  }

  [BurstCompile]
  public struct PickupOnTriggerSystemJob : ITriggerEventsJob
  {
    [ReadOnly] public ComponentDataFromEntity<PickupTag> allPickups;
    public ComponentDataFromEntity<PlayerTag> allPlayers;
    public EntityCommandBuffer ecb;

    public void Execute(TriggerEvent triggerEvent)
    {
      Entity entityA = triggerEvent.EntityA;
      Entity entityB = triggerEvent.EntityB;

      if (!allPickups.HasComponent(entityA) && !allPickups.HasComponent(entityB)) return;
      if (allPickups.HasComponent(entityA) && allPickups.HasComponent(entityB)) return;

      if (allPlayers.HasComponent(entityA) && allPickups.HasComponent(entityB))
      {
        ecb.DestroyEntity(entityB);
      }
      else if (allPlayers.HasComponent(entityB) && allPickups.HasComponent(entityA))
      {
        ecb.DestroyEntity(entityA);
      }

    }
  }
}