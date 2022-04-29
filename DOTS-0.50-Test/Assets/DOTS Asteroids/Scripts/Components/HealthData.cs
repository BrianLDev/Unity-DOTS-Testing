using Unity.Entities;

[GenerateAuthoringComponent]
public struct HealthData : IComponentData
{
  public float health;
  public float damageCaused;
  public bool isDead;
}