using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateAfter(typeof(TransformSystemGroup))] // makes sure this runs after everything in TransformSystemGroup (which is later in loop)
public partial class TurnToTargetSystem : SystemBase
{
    protected override void OnUpdate()
    {
      float deltaTime = Time.DeltaTime;
      
      Entities.
        WithAll<ChaserTag>().
        ForEach((ref Rotation rot, ref MoveData moveData, in Translation pos, in TargetData targetData) => 
        {
        // Note - use GetComponentDataFromEntity to get all translation data and store it in allTranslations
        ComponentDataFromEntity<Translation> allTranslations = GetComponentDataFromEntity<Translation>(true); // true == readOnly
          Translation targetPos = allTranslations[targetData.targetEntity];
          float3 dirToTarget = targetPos.Value - pos.Value;
          moveData.direction = dirToTarget;
          if (!moveData.direction.Equals(float3.zero))
          {
            quaternion targetRotation = quaternion.LookRotationSafe(moveData.direction, math.back());
            rot.Value = math.slerp(rot.Value, targetRotation, moveData.turnSpeed * deltaTime);
          }
        }).ScheduleParallel(); 
    }
}
