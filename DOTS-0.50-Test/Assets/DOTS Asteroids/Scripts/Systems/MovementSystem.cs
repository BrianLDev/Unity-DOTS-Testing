using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial class MovementSystem : SystemBase
{
protected override void OnUpdate()
  {
    float deltaTime = Time.DeltaTime;

    // Player and chasers (thruster movement)
    Entities.
      WithAny<PlayerTag, ChaserTag>().
      ForEach((ref Translation pos, in MoveData moveData, in Rotation rot) => 
      {
        float3 forwardDirection = math.forward(rot.Value);
        pos.Value += forwardDirection * moveData.speed * deltaTime;
      }).ScheduleParallel();

    // Asteroids (constant movement)
    Entities.
      WithAll<AsteroidTag>().
      ForEach((ref Translation pos, in MoveData moveData) =>
      {
        pos.Value += moveData.moveDirection * moveData.speed * deltaTime;
      }).ScheduleParallel();
  }
}