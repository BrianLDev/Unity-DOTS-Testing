using UnityEngine;
using System;
using Unity.Entities;
using Unity.Mathematics;

public partial class PlayerInputSystem : SystemBase
{
protected override void OnUpdate()
  {
    Entities.ForEach((ref MoveData moveData, in InputData inputData) => 
    {
      // Input to float3 - My version
      moveData.moveDirection = float3.zero;
      moveData.moveDirection += math.up() * Convert.ToInt32(Input.GetKey(inputData.upKey));
      moveData.moveDirection += math.down() * Convert.ToInt32(Input.GetKey(inputData.downKey));
      moveData.moveDirection += math.right() * Convert.ToInt32(Input.GetKey(inputData.rightKey));
      moveData.moveDirection += math.left() * Convert.ToInt32(Input.GetKey(inputData.leftKey));
      moveData.moveDirection = math.normalizesafe(moveData.moveDirection);

      // Input to float3 - Wilmer's version
      // bool isUpKeyPressed = Input.GetKey(inputData.upKey);
      // bool isDownKeyPressed = Input.GetKey(inputData.downKey);
      // bool isRightKeyPressed = Input.GetKey(inputData.rightKey);
      // bool isLeftKeyPressed = Input.GetKey(inputData.leftKey);

      // moveData.direction.y = isUpKeyPressed ? 1 : 0;
      // moveData.direction.y += isDownKeyPressed ? -1 : 0;
      // moveData.direction.x = isRightKeyPressed ? 1 : 0;
      // moveData.direction.x += isLeftKeyPressed ? -1 : 0;

    }).Run();
  }
}