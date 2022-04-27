using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial class TurnToRotationSystem : SystemBase
{
  protected override void OnUpdate()
  {
    float deltaTime = Time.DeltaTime;

    // Turn the player
    Entities.
      WithAll<PlayerTag>().
      ForEach((ref Rotation rot, in MoveData moveData) =>
      {
        if (!moveData.direction.Equals(float3.zero))
        {
          quaternion targetRotation = quaternion.LookRotationSafe(moveData.direction, math.back());
          rot.Value = math.slerp(rot.Value, targetRotation, moveData.turnSpeed * deltaTime);
        }
      }).Schedule();  // Schedule runs on 1 worker thread (not main thread)
  }

}