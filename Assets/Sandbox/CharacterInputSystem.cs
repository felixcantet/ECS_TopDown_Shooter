using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class CharacterInputSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.WithAll<CharacterInputData>().ForEach( (ref CharacterInputData inputData) =>
        {
            inputData = new CharacterInputData(){
                MouseDown = Input.GetKeyDown(KeyCode.Mouse0),
                MouseUp = Input.GetKeyUp(KeyCode.Mouse0),
                MousePress = Input.GetKey(KeyCode.Mouse0),

                MousePosition = new Unity.Mathematics.float2(Input.mousePosition.x, Input.mousePosition.y),
                Horizontal = Input.GetAxis("Horizontal"),
                Vertical = Input.GetAxis("Vertical")
            };
        });
    }
}
