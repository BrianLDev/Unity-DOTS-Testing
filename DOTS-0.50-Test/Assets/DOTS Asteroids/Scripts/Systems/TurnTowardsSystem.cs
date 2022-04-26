using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial class TurnTowardsSystem : SystemBase
{
  protected override void OnUpdate()
  {
    float deltaTime = Time.DeltaTime;

    Entities.
      WithAll<PlayerTag>().
      ForEach((ref Rotation rot, in MoveData moveData) =>
    {
      if (!moveData.direction.Equals(float3.zero))
      {
        quaternion targetRotation = quaternion.LookRotationSafe(moveData.direction, math.back());
        rot.Value = math.slerp(rot.Value, targetRotation, moveData.turnSpeed * deltaTime);
      }
    }).Schedule();
  }
}