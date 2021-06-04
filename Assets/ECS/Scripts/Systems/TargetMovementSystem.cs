using Unity.Entities;
using Unity.Jobs;

[AlwaysSynchronizeSystem]
public class TargetMovementSystem : JobComponentSystem
{

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities
        .WithAll<TargetData>()
        // .WithoutBurst()
        .ForEach((ref Unity.Transforms.Translation position) =>
        {
            var pos = HandBehaviour.instance.transform.position;
            position.Value = pos;

        }).Run();

        return default;
    }
}

