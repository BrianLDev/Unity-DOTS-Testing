using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct SpinnerData : IComponentData
{
  public float3 spinRotation;
}