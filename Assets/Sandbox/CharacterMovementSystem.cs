using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Burst;
using Unity.Physics;
using Unity.Collections;

[UpdateAfter(typeof(CharacterInputSystem))]
public class CharacterMovementSystem : JobComponentSystem
{

    [BurstCompile][RequireComponentTag(typeof(CharacterTag))]
    public struct CharacterMovementJob : IJobForEach<CharacterMovementData, PhysicsVelocity, CharacterInputData>
    {
        public float deltaTime;

        public void Execute(ref CharacterMovementData characterMovement, ref PhysicsVelocity velocity, [ReadOnly] ref CharacterInputData inputs)
        {
            var v = new PhysicsVelocity();
            var targetVel = new float3(inputs.Horizontal, 0f, inputs.Vertical) * characterMovement.movementSpeed;
            v.Linear = math.lerp(velocity.Linear, targetVel, 1f - math.exp(-characterMovement.movementSharpness *  deltaTime));
            v.Linear.y = 0;
            velocity = v;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new CharacterMovementJob();
        job.deltaTime = Time.DeltaTime;
        inputDeps = job.Schedule(this, inputDeps);
        return inputDeps;
    }
}
