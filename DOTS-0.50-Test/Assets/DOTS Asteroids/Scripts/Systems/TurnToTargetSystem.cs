using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using Unity.Physics.Extensions;

public partial class TurnToTargetSystem : SystemBase
{
  protected override void OnUpdate()
  {
    float deltaTime = Time.DeltaTime;
    
    Entities.
      WithAll<ChaserTag>().
      ForEach((ref Rotation rot, ref MoveData moveData, ref PhysicsVelocity physicsVelocity, in Translation pos, in TargetData targetData, in PhysicsMass physicsMass) => 
      {
        if (targetData.targetEntity.Equals(Entity.Null))
          return;
        else
        {
          // Note - use GetComponentDataFromEntity to get all translation data and store it in allTranslations
          ComponentDataFromEntity<Translation> allTranslations = GetComponentDataFromEntity<Translation>(true); // true == readOnly
          Translation targetPos = allTranslations[targetData.targetEntity];
          float3 dirToTarget = targetPos.Value - pos.Value;
          moveData.moveDirection = dirToTarget;
          if (!moveData.moveDirection.Equals(float3.zero))
          {
            quaternion targetRotation = quaternion.LookRotationSafe(moveData.moveDirection, math.back());
            rot.Value = math.slerp(rot.Value, targetRotation, moveData.turnSpeed * deltaTime);
            physicsVelocity.SetAngularVelocityWorldSpace(physicsMass, rot, float3.zero);
          }
        }
      }).ScheduleParallel();
    
    Dependency.Complete();
  }
}
