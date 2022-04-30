using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using UnityEngine;

public partial class TurnToRotationSystem : SystemBase
{
  protected override void OnUpdate()
  {
    float deltaTime = Time.DeltaTime;

    // Turn the player
    Entities.
      WithAll<PlayerTag>().
      ForEach((ref Rotation rot, ref PhysicsVelocity physicsVelocity, in MoveData moveData, in PhysicsMass physicsMass) =>
      {
        if (!moveData.moveDirection.Equals(float3.zero))
        {
          quaternion targetRotation = quaternion.LookRotationSafe(moveData.moveDirection, math.back());
          rot.Value = math.slerp(rot.Value, targetRotation, moveData.turnSpeed * deltaTime);
          physicsVelocity.SetAngularVelocityWorldSpace(physicsMass, rot, float3.zero);
        }
      }).Schedule();  // Schedule runs on 1 worker thread (not main thread)
  }

}