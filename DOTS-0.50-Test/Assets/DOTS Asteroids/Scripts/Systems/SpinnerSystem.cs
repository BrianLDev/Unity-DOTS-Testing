using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

// public partial class SpinnerSystem : SystemBase
// {
//   protected override void OnUpdate()
//   {
//     float deltaTime = Time.DeltaTime;
    
//     Entities.
//       ForEach((ref Rotation rot, in SpinnerData spinnerData) => 
//     {
//       quaternion angleToRotate = quaternion.Euler(spinnerData.spinRotation * deltaTime);
//       rot.Value = math.mul(rot.Value, angleToRotate);
//     }).ScheduleParallel();
//   }
// }
