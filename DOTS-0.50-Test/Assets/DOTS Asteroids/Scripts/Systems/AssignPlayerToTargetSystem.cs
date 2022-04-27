using UnityEngine;
using Unity.Entities;

// [UpdateInGroup(typeof(InitializationSystemGroup))]  // this forces the system to run earlier with the Initialization groups
[UpdateBefore(typeof(TurnToTargetSystem))]  // makes sure this script is run before TurnToTargetSystem
public partial class AssignPlayerToTargetSystem : SystemBase
{
  // Note - we only want this script to run once, so we run the code in OnStartRunning() instead of OnUpdate()
  protected override void OnStartRunning()
  {
    base.OnStartRunning();
    // Note - To get the playerEntity and data, create an EntityQuery using GetEntityQuery to gather entities that match certain criteria
    // EntityQuery playerQuery = GetEntityQuery(typeof(PlayerTag));
    EntityQuery playerQuery = GetEntityQuery(ComponentType.ReadOnly<PlayerTag>());  // readonly version of the above line (slightly faster)

    // Note - use .ToEntityArray if we want to get an array of Entities
    // Entity playerEntity = playerQuery.ToEntityArray(Unity.Collections.Allocator.Temp)[0];

    // Note - use .GetSingletonEntity if we just want to get 1 entity
    Entity playerEntity = playerQuery.GetSingletonEntity();

    Entities.
      WithAll<ChaserTag>().
      ForEach((ref TargetData targetData) =>
      {
        if (playerEntity != Entity.Null)  // Entities need to check against Entity.Null instead of regular null
        {
          targetData.targetEntity = playerEntity;
        }
      }).Run();
  }

  protected override void OnUpdate()
  {
    // OnUpdate() is required for all systems but in this case it isn't needed so we just leave it blank
  }
}