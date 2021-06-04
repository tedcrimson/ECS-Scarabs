using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;
using Unity.Transforms;
using Unity.Physics.Extensions;
using Unity.Collections;
using Unity.Physics.Systems;

// [AlwaysSynchronizeSystem]
// [UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateBefore(typeof(BuildPhysicsWorld))]
public class MovementSystem : SystemBase
{

    // protected override JobHandle OnUpdate(JobHandle inputDeps)
    // {
    //     float deltaTime = Time.DeltaTime;
    //     ComponentDataFromEntity<LocalToWorld> localToWorldFromEntityToFollow = GetComponentDataFromEntity<LocalToWorld>(true);

    //     float2 curInput = new float2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

    //     Entities.ForEach((ref PhysicsVelocity vel, ref Unity.Transforms.Rotation rotation, ref Unity.Transforms.Translation position, in SpeedData speedData) =>
    //     {
    //         float3 toPos = localToWorldFromEntityToFollow[speedData.entityToFollow].Position;
    //         float2 newVel = vel.Linear.xz;
    //         float3 vec = math.normalize(toPos - position.Value);
    //         newVel = (vec * speedData.speed * deltaTime).xz;

    //         // newVel += curInput * speedData.speed * deltaTime;

    //         vel.Linear.xz = newVel;
    //         vec.y = 0;
    //         rotation.Value = Quaternion.LookRotation(vec);
    //     }).Run();

    //     return default;
    // }

    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        // float2 curInput = new float2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Entities.
        WithBurst().
        ForEach((ref PhysicsVelocity vel, ref PhysicsMass mass, ref Unity.Transforms.Rotation rotation, ref Unity.Transforms.Translation position, in SpeedData speedData) =>
        {
            ComponentDataFromEntity<LocalToWorld> localToWorldFromEntityToFollow = GetComponentDataFromEntity<LocalToWorld>(true);
            float3 toPos = localToWorldFromEntityToFollow[speedData.entityToFollow].Position;
            float3 vec = toPos - position.Value;
            // vec.y = 0;
            float2 newVel = vel.Linear.xz;
            float3 dir = math.normalize(vec);
            newVel = (dir * speedData.speed * deltaTime).xz;

            // newVel += curInput * speedData.speed * deltaTime;
            // vel.ApplyLinearImpulse(mass, dir * speedData.speed * deltaTime);
            // vel.Linear.xz = newVel;
            // dir.y = 0;
            float magnitude = Vector3.Magnitude(vec);
            if (magnitude < 2 && position.Value.y < 0.3f)
            {
                position.Value = position.Value + dir * -3 * deltaTime;
            }
            else
                vel.ApplyLinearImpulse(mass, dir * speedData.speed * magnitude * deltaTime);
            rotation.Value = Quaternion.Lerp(rotation.Value, Quaternion.LookRotation(dir), Mathf.Max(0, 4f - magnitude) / 4f);
            // rotation.Value = Quaternion.RotateTowards(rotation.Value, Quaternion.LookRotation(dir), delta);
        }).ScheduleParallel();
    }
}

