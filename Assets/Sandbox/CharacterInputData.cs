
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct CharacterInputData : IComponentData
{
    public float2 MousePosition;
    public float Horizontal;
    public float Vertical;

    public bool MouseDown;
    public bool MouseUp;

    public bool MousePress;
}
