using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

public partial class RestrictTo2DSystem : SystemBase
{
  protected override void OnUpdate()
  {
    Entities.ForEach((ref Translation pos, ref PhysicsVelocity physicsVelocity) =>
    {
      if (pos.Value.z != 0)
        pos.Value.z = 0;
      if (physicsVelocity.Linear.z != 0)
        physicsVelocity.Linear.z = 0;
    }).ScheduleParallel();
  }
}