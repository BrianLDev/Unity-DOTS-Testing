using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

// NOTE: THIS IS SINGLE-THREADED BUT SHOWS THE GENERAL STRUCTURE AND WORKFLOW FOR DOTS BASED SYSTEMS / JOBS
public class WaveSystem : ComponentSystem
{
  protected override void OnUpdate()
  {
    Entities.ForEach((ref Translation trans, ref MoveSpeedData moveSpeed, ref WaveData waveData) => 
    {
      float zPos = waveData.amplitude * math.sin( 
        (float)Time.ElapsedTime * moveSpeed.Value + trans.Value.x * waveData.xOffset + trans.Value.y * waveData.yOffset);
      trans.Value = new float3(trans.Value.x, trans.Value.y, zPos);
    });
  }
}