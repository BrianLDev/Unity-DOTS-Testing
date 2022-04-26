using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class SpinnerSystem : SystemBase
{
  protected override void OnUpdate()
  {
    float deltaTime = Time.DeltaTime;
    float3 rotAngle = math.normalizesafe(new float3(1,1,0));
    
    Entities.
      WithAll<AsteroidTag>().
      WithNone<PlayerTag>().
      ForEach((ref Rotation rot, in MoveData moveData) => 
    {
      quaternion angleToRotate = quaternion.Euler(rotAngle * moveData.turnSpeed * deltaTime);
      rot.Value = math.mul(rot.Value, angleToRotate);
    }).ScheduleParallel();
  }
}
