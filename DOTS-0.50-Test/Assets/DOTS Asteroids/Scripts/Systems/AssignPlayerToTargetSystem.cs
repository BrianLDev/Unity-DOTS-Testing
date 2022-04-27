using Unity.Entities;

// This script assigns the Player as the target for all Chasers
// Note - not very efficient since it does it on every single update, but this is just for practice not for a real game
public partial class AssignPlayerToTargetSystem : SystemBase
{
  protected override void OnUpdate()
  {
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
      }).ScheduleParallel();
  }
}