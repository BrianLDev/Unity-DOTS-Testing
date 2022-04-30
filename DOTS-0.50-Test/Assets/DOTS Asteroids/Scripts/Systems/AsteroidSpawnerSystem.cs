using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial class AsteroidSpawnerSystem : SystemBase
{
  protected override void OnStartRunning()
  {
    base.OnStartRunning();
    uint seed = 654321;
    Random rand = new Random(seed);
    float3 screenDimensions = new float3(30, 16, 0);

    Entities.
      WithAll<AsteroidTag>().
      ForEach((ref Translation pos, ref CompositeScale scale, ref PhysicsVelocity physicsVelocity, ref PhysicsMass physicsMass) =>
      {
        pos.Value = (rand.NextFloat3()-0.5f) * screenDimensions;
        pos.Value.z = 0;
        scale.Value = (rand.NextFloat()+0.5f) * 2f * float4x4.identity;
        scale.Value.c3.w = 1;
        physicsVelocity.Linear = rand.NextFloat3Direction()-0.5f;
        physicsVelocity.Linear.z = 0;
        physicsVelocity.Angular = rand.NextFloat3Direction()-0.5f;
        physicsMass.InverseMass = 1/(scale.Value.c0.x * 50);
      }).ScheduleParallel();

    this.Dependency.Complete();
  }

  protected override void OnUpdate()
  {
  }
}
