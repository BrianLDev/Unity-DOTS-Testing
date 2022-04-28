using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial class MoveWorldPosSystem : SystemBase
{
protected override void OnUpdate()
  {
    float deltaTime = Time.DeltaTime;

    Entities.
      WithAll<AsteroidTag>().
      ForEach((ref Translation pos, ref Rotation rot, in MoveData moveData) =>
      {
        quaternion prevRot = rot.Value;
        rot.Value = quaternion.identity;
        pos.Value += moveData.moveDirection * moveData.speed * deltaTime;
        rot.Value = prevRot;
      }).ScheduleParallel();
  }
}