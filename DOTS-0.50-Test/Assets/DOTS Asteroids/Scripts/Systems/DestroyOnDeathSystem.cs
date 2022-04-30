using Unity.Entities;
using Unity.Physics.Systems;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateBefore(typeof(BuildPhysicsWorld))]
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
      if (healthData.isDead && !entity.Equals(Entity.Null))
      {
        ecb.DestroyEntity(entity);
      }
    }).Run();

    this.Dependency.Complete();
    commandBufferSystem.AddJobHandleForProducer(this.Dependency);
  }
}