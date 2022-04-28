using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using System.Diagnostics;

public partial class PickupOnTriggerSystem : SystemBase
{
  // private BuildPhysicsWorld buildPhysicsWorld;  // setting up colliders and entities
  private StepPhysicsWorld stepPhysicsWorld;    // running simulation

  protected override void OnCreate()
  {
    base.OnCreate();
    // buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
    stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
  }

  protected override void OnUpdate()
  {
    Dependency = new PickupOnTriggerSystemJob
    {
      allPickups = GetComponentDataFromEntity<PickupTag>(true),
      allPlayers = GetComponentDataFromEntity<PlayerTag>(true)
    }.Schedule(stepPhysicsWorld.Simulation, Dependency);
  }

  [BurstCompile]
  public struct PickupOnTriggerSystemJob : ITriggerEventsJob
  {
    [ReadOnly] public ComponentDataFromEntity<PickupTag> allPickups;
    [ReadOnly] public ComponentDataFromEntity<PlayerTag> allPlayers;

    public void Execute(TriggerEvent triggerEvent)
    {
      Entity entityA = triggerEvent.EntityA;
      Entity entityB = triggerEvent.EntityB;

      if ((allPlayers.HasComponent(entityA) && allPickups.HasComponent(entityB)) ||
          (allPlayers.HasComponent(entityB) && allPickups.HasComponent(entityA)))
      {
        UnityEngine.Debug.Log("Player collided with Pickup");
      }

    }
  }
}