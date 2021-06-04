using Unity.Entities;

[GenerateAuthoringComponent]
public struct SpeedData : IComponentData
{
    public Entity entityToFollow;
    public float speed;
    // public int counter;
}