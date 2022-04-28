using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(EndFramePhysicsSystem))]
public partial class PickupOnTriggerSystem : SystemBase
{
  // private BuildPhysicsWorld buildPhysicsWorld;  // setting up colliders and entities
  private StepPhysicsWorld stepPhysicsWorld;    // running simulation
  private EndFixedStepSimulationEntityCommandBufferSystem commandBufferSystem;

  protected override void OnCreate()
  {
    base.OnCreate();
    // buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
    stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    commandBufferSystem = World.GetOrCreateSystem<EndFixedStepSimulationEntityCommandBufferSystem>();
  }

  protected override void OnUpdate()
  {
    Dependency = new PickupOnTriggerSystemJob
    {
      allPickups = GetComponentDataFromEntity<PickupTag>(true),
      allPlayers = GetComponentDataFromEntity<PlayerTag>(true),
      entityCommandBuffer = commandBufferSystem.CreateCommandBuffer()
    }.Schedule(stepPhysicsWorld.Simulation, Dependency);
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
        entityCommandBuffer.DestroyEntity(entityB);
      else if (allPlayers.HasComponent(entityB) && allPickups.HasComponent(entityA))
        entityCommandBuffer.DestroyEntity(entityA);

    }
  }
}