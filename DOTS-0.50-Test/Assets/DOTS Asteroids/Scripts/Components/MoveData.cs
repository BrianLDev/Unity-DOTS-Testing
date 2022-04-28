using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct MoveData : IComponentData
{
  public float3 moveDirection;
  public float speed;
  public float turnSpeed;
}