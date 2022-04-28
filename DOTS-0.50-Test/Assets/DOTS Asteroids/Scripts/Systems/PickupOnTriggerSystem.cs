using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;

// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
// [UpdateAfter(typeof(EndFramePhysicsSystem))]
public partial class PickupOnTriggerSystem : SystemBase
{
  // private BuildPhysicsWorld buildPhysicsWorld;  // setting up colliders and entities
  private StepPhysicsWorld stepPhysicsWorld;    // running simulation
  private EndSimulationEntityCommandBufferSystem commandBufferSystem;

  protected override void OnCreate()
  {
    base.OnCreate();
    // buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
    stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
  }

  protected override void OnUpdate()
  {
    PickupOnTriggerSystemJob pickupOnTriggerSystemJob = new PickupOnTriggerSystemJob
    {
      allPickups = GetComponentDataFromEntity<PickupTag>(true),
      allPlayers = GetComponentDataFromEntity<PlayerTag>(true),
      entityCommandBuffer = commandBufferSystem.CreateCommandBuffer()
    };

    Dependency = pickupOnTriggerSystemJob.Schedule(stepPhysicsWorld.Simulation, Dependency);

    commandBufferSystem.AddJobHandleForProducer(Dependency);
    Dependency.Complete();
  }

  [BurstCompile]
  public struct PickupOnTriggerSystemJob : ITriggerEventsJob
  {
    [ReadOnly] public ComponentDataFromEntity<PickupTag> allPickups;
    [ReadOnly] public ComponentDataFromEntity<PlayerTag> allPlayers;
    public EntityCommandBuffer entityCommandBuffer;

    public void Execute(TriggerEvent triggerEvent)
    {
      Entity entityA = triggerEvent.EntityA;
      Entity entityB = triggerEvent.EntityB;

      if (allPlayers.HasComponent(entityA) && allPickups.HasComponent(entityB))
      {
        entityCommandBuffer.DestroyEntity(entityB);
      }
      else if (allPlayers.HasComponent(entityB) && allPickups.HasComponent(entityA))
      {
        entityCommandBuffer.DestroyEntity(entityA);
      }

    }
  }
}