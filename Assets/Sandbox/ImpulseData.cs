using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Physics;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct ImpulseData : IComponentData
{
    public float3 Impulse;
}
