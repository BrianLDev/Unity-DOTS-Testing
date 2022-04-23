using UnityEngine;
using Unity.Entities;

// NOTE: THIS USES THE OLD INPUT SYSTEM BUT CAN BE MODIFIED TO USE THE NEW INPUT SYSTEM

[GenerateAuthoringComponent]
public struct InputData : IComponentData
{
  public KeyCode upKey;
  public KeyCode downKey;
  public KeyCode rightKey;
  public KeyCode leftKey;
}
