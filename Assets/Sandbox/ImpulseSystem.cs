using System.Collections;
using System.Collections.Generic;
//using UnityEngine;
using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Physics.Systems;
using Unity.Rendering;



[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class ImpulseSystem : JobComponentSystem
{
    BuildPhysicsWorld buildPhysicsWorldSystem;
    StepPhysicsWorld stepPhysicsWorldSystem;
   


    protected override void OnCreate(){
        buildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorldSystem = World.GetOrCreateSystem<StepPhysicsWorld>();
    }
    [BurstCompile]
    struct TriggerObjects : ITriggerEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<TriggerObject> objects;
        public ComponentDataFromEntity<PhysicsVelocity> velocityGroup;
        public ComponentDataFromEntity<MaterialColor> colorGroup;

        public void Execute(TriggerEvent triggerEvent){
            Entity entityA = triggerEvent.Entities.EntityA;
            Entity entityB = triggerEvent.Entities.EntityB;

            bool isBodyATrigger = objects.Exists(entityA);
            bool isBodyBTrigger = objects.Exists(entityB);

            // if(!isBodyATrigger){

            // var c = velocityGroup[entityA];
            // c.Linear = new float3(0, 1.5f, 0);
            // velocityGroup[entityA] = c;
            // }

            // if(!isBodyBTrigger){

            // var d = velocityGroup[entityB];
            // d.Linear = new float3(0, 1.5f, 0);
            // velocityGroup[entityB] = d;
            // }

            if(isBodyATrigger && isBodyBTrigger)
                return;
            
            bool isBodyADynamic = velocityGroup.Exists(entityA);
            bool isBodyBDynamic = velocityGroup.Exists(entityB);

            if ((isBodyATrigger && !isBodyBDynamic) ||
                (isBodyBTrigger && !isBodyADynamic))
                return;
            
            var triggerEntity = isBodyATrigger? entityA : entityB;
            var dynamicEntity = isBodyATrigger? entityB : entityA;

            //var velocity = velocityGroup[dynamicEntity];
            {
            var component = velocityGroup[dynamicEntity]; 
            component.Linear = component.Linear + new float3(0.0f, 10f, 0.0f);
            velocityGroup[dynamicEntity] = component;
            
            var color = colorGroup[dynamicEntity]; 
            color.Value = new float4(1, 0,0,1);
            colorGroup[dynamicEntity] = color;

            }
            
            
        }
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        if(UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Mouse0)){

        UnityEngine.Debug.Log("System Updated");
        JobHandle jobHandle = new TriggerObjects{
            objects = GetComponentDataFromEntity<TriggerObject>(true),
            velocityGroup = GetComponentDataFromEntity<PhysicsVelocity>(),
            colorGroup = GetComponentDataFromEntity<MaterialColor>(),
        }.Schedule(stepPhysicsWorldSystem.Simulation, ref buildPhysicsWorldSystem.PhysicsWorld, inputDeps);
        return jobHandle;
        }
        return default;
    }
}
