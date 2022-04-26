using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public partial class MovementSystem : SystemBase
{
protected override void OnUpdate()
  {
    float deltaTime = Time.DeltaTime;

    Entities.ForEach((ref Translation pos, in MoveData moveData) => 
    {
      pos.Value += moveData.direction * moveData.speed * deltaTime;
    }).Run();
  }
}