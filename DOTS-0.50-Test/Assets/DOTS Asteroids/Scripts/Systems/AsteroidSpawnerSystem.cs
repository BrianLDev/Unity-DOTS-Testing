using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial class AsteroidSpawnerSystem : SystemBase
{
  protected override void OnStartRunning()
  {
    base.OnStartRunning();
    uint seed = 654321;
    Random rand = new Random(seed);
    float3 screenDimensions = new float3(35, 20, 0);

    Entities.
      WithAll<AsteroidTag>().
      ForEach((ref Translation pos, ref NonUniformScale scale, ref MoveData moveData, ref SpinnerData spinnerData) =>
      {
        pos.Value = (rand.NextFloat3()-0.5f) * (screenDimensions);
        scale.Value = (rand.NextFloat3()+0.5f) * 2f;
        moveData.speed = rand.NextFloat();
        moveData.moveDirection = rand.NextFloat3Direction();
        moveData.moveDirection.z = 0;
        // spinnerData.spinRotation = rand.NextFloat3Direction();
        spinnerData.spinRotation.z = (rand.NextFloat()-0.5f);
      }).Run();
  }

  protected override void OnUpdate()
  {
  }
}
