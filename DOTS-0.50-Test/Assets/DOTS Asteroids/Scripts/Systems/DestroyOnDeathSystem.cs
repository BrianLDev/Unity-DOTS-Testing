using Unity.Entities;
using Unity.Transforms;

[UpdateBefore(typeof(TransformSystemGroup))]
public partial class DestroyOnDeathSystem : SystemBase
{
  private EndSimulationEntityCommandBufferSystem commandBufferSystem;

  protected override void OnCreate()
  {
    base.OnCreate();
    commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
  }

  protected override void OnUpdate()
  {
    EntityCommandBuffer ecb = commandBufferSystem.CreateCommandBuffer();

    Entities.
      WithAny<PlayerTag, ChaserTag, AsteroidTag>().
      ForEach((Entity entity, in HealthData healthData) =>
    {
      if (healthData.isDead)
      {
        ecb.DestroyEntity(entity);
      }
    }).Schedule();

    commandBufferSystem.AddJobHandleForProducer(this.Dependency);
  }
}