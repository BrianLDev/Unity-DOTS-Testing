using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial class TurnTowardsSystem : SystemBase
{
  protected override void OnUpdate()
  {
    float deltaTime = Time.DeltaTime;

    // Turn the player
    Entities.
      WithAll<PlayerTag>().
      ForEach((ref Rotation rot, in MoveData moveData) =>
      {
        TurnTowards(ref rot, moveData, deltaTime);
      }).Schedule();  // Schedule runs on 1 worker thread (not main thread)

    // Turn the chasers
    Entities.
      WithAll<ChaserTag>().
      WithNone<PlayerTag>().  // probably don't need this but it makes extra sure that player is not updated here
      ForEach((ref Rotation rot, ref MoveData moveData, in Translation pos, in TargetData targetData) => 
      {
        // get all translation data and store it in allTranslations
        ComponentDataFromEntity<Translation> allTranslations = GetComponentDataFromEntity<Translation>(true); // true == readOnly
        Translation targetPos = allTranslations[targetData.targetEntity];
        float3 dirToTarget = targetPos.Value - pos.Value;
        moveData.direction = dirToTarget;
        TurnTowards(ref rot, moveData, deltaTime);
      }).ScheduleParallel(); 
  }

  public static void TurnTowards(ref Rotation rot, in MoveData moveData, in float deltaTime)
  {
    if (!moveData.direction.Equals(float3.zero))
    {
      quaternion targetRotation = quaternion.LookRotationSafe(moveData.direction, math.back());
      rot.Value = math.slerp(rot.Value, targetRotation, moveData.turnSpeed * deltaTime);
    }
  }
}