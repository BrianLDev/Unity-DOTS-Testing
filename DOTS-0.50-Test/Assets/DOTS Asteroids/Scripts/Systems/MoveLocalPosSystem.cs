using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial class MoveLocalPosSystem : SystemBase
{
protected override void OnUpdate()
  {
    float deltaTime = Time.DeltaTime;

    Entities.
      WithAny<PlayerTag, ChaserTag>().
      ForEach((ref Translation pos, in MoveData moveData, in Rotation rot) => 
      {
        float3 forwardDirection = math.forward(rot.Value);
        pos.Value += forwardDirection * moveData.speed * deltaTime;
      }).ScheduleParallel();

  }
}