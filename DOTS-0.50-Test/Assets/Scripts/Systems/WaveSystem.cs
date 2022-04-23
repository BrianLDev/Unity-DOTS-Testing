using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Jobs;
using UnityEngine;

// CURRENT MULTI-THREADED VERSION (SystemBase)
public partial class WaveSystem : SystemBase
{
  protected override void OnUpdate()
  {
    float elapsedTime = (float)Time.ElapsedTime;
    Entities.ForEach((ref Translation trans, in MoveSpeedData moveSpeed, in WaveData waveData) => 
    {
      float zPos = waveData.amplitude * math.sin( 
        elapsedTime * moveSpeed.Value + trans.Value.x * waveData.xOffset + trans.Value.y * waveData.yOffset);
      trans.Value = new float3(trans.Value.x, trans.Value.y, zPos);
    }).ScheduleParallel();
  }
}

/* NOTES FOR SYSTEMBASE:
Automatically creates an assumed Jobhandle for every .ForEach job, but can also explicitly add one if you have dependencies that require it.

DIFFERENCE BETWEEN SYSTEMBASE JOB PROCESS OPTIONS
.Run() = single thread on main thread
.Schedule() = single thread on a worker thread (not on main thread)
.ScheduleParallel() = multi threaded on worker threads

If there are any dependencies, put the jobHandle in the parens.

Note that synch points (job handle.complete(), other job dependancy and entity command buffers) can force the execution on main thread to finish the work.
*/

// OLD, DEPRECATED MULTI-THREADED VERSION (JobComponentSystem)
// public class WaveSystem : JobComponentSystem
// {
//   protected override JobHandle OnUpdate(JobHandle inputDependencies)
//   {
//     float elapsedTime = (float)Time.ElapsedTime;
//     JobHandle jobHandle = Entities.ForEach((ref Translation trans, in MoveSpeedData moveSpeed, in WaveData waveData) => 
//     {
//       float zPos = waveData.amplitude * math.sin( 
//         elapsedTime * moveSpeed.Value + trans.Value.x * waveData.xOffset + trans.Value.y * waveData.yOffset);
//       trans.Value = new float3(trans.Value.x, trans.Value.y, zPos);
//     }).Schedule(inputDependencies);

//     return jobHandle;
//   }
// }

// SINGLE THREADED VERSION
// public class WaveSystem : ComponentSystem
// {
//   protected override void OnUpdate()
//   {
//     Entities.ForEach((ref Translation trans, ref MoveSpeedData moveSpeed, ref WaveData waveData) => 
//     {
//       float zPos = waveData.amplitude * math.sin( 
//         (float)Time.ElapsedTime * moveSpeed.Value + trans.Value.x * waveData.xOffset + trans.Value.y * waveData.yOffset);
//       trans.Value = new float3(trans.Value.x, trans.Value.y, zPos);
//     });
//   }
// }