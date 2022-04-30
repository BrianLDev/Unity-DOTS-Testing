using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;

public partial class MovementSystem : SystemBase
{
protected override void OnUpdate()
  {
    float deltaTime = Time.DeltaTime;

    Entities.
      WithAny<PlayerTag, ChaserTag>().
      ForEach((ref PhysicsVelocity physicsVelociy, in MoveData moveData, in Rotation rot) => 
      {
        float3 forwardDirection = math.forward(rot.Value);
        if (!moveData.moveDirection.Equals(float3.zero))
          physicsVelociy.Linear += forwardDirection * moveData.speed * deltaTime;
      }).ScheduleParallel();

  }
}