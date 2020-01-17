using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Jobs;
using Unity.Burst;
using Unity.Transforms;
using Unity.Physics.Systems;

[UpdateAfter(typeof(CharacterInputSystem))]
public class CharacterRotationSystem : JobComponentSystem
{
    [RequireComponentTag(typeof(CharacterTag))][BurstCompile]
    public struct CharacterRotationJob : IJobForEach<CharacterMovementData, CharacterInputData, Rotation, Translation>
    {
        [ReadOnly]
        public PhysicsWorld physicsWorld;
        public RaycastInput input;
        public float deltaTime;
        public void Execute([ReadOnly]ref CharacterMovementData movementData, [ReadOnly] ref CharacterInputData inputs, ref Rotation rotation, [ReadOnly] ref Translation translation)
        {
            if(physicsWorld.CastRay(input, out var hit))
            {
                float3 hitPos = new float3(hit.Position.x, translation.Value.y, hit.Position.z);
                var target = quaternion.LookRotationSafe(math.normalize(hitPos - translation.Value), math.up());
                var newRot = new Rotation(){
                    Value = math.slerp(rotation.Value, target, 1f - math.exp(-movementData.rotationSharpness * deltaTime * movementData.rotationSpeed))
                    
                };
                
                rotation = newRot;
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new CharacterRotationJob();
        UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        job.input = new RaycastInput(){
            Start = ray.origin,
            End = ray.GetPoint(100),
            // Filter = new CollisionFilter(){
            //     BelongsTo = ~0u,
            //     CollidesWith = ~0u,
            //     GroupIndex = 0
            // }
            Filter = CollisionFilter.Default
        };
        job.physicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>().PhysicsWorld;
        job.deltaTime = Time.DeltaTime;
        inputDeps = job.Schedule(this, inputDeps);
        
        return inputDeps;
    }
}
