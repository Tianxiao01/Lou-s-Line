using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision_river : Collision_normalObs
{
    public override IEnumerator CollisionBehavior(){
        Renderer renderer=GetComponent<Renderer>();
        Color originColor = renderer.material.color;
        renderer.material.color=new Color(0f, 0.8f, 1f, 1f);
        yield return new WaitForSeconds(0.075f);
        renderer.material.color=originColor;
        yield return new WaitForSeconds(0.075f);
        renderer.material.color=new Color(0f, 0.8f, 1f, 1f);
        yield return new WaitForSeconds(0.075f);
        renderer.material.color=originColor;
        isCollisionBehaviorCoroutineRunning=false;
    }
}
