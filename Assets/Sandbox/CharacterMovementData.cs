using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct CharacterMovementData : IComponentData
{
    public float movementSpeed;
    public float rotationSpeed;
    public float movementSharpness;
    public float rotationSharpness;
}
