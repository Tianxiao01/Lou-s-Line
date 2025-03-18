using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collision_behavior : Collision_normalObs
{

   public override IEnumerator CheckCollision(){ 
        while(true){
            float distance=CalculateDistance(transform.position,playerTransform.position);
            if(playerstate.BoosterState=="Small_Size"){
                if (distance<(0.25f+0.25f)){
                    isCollided=true;
                }
                else{
                    isCollided=false;
                }
            }else{
                if (distance<(0.5f+0.25f)){
                    isCollided=true;
                }
                else{
                    isCollided=false;
                }
            } 
            
            yield return null;   
        }                           // check collision every frame   
    }


    public override IEnumerator CollisionBehavior(){
        //Debug.Log("collision");
        transform.position=new Vector3(-35f,22f,0.2f); // the Booster need to be assured to not be deactivated so fast because the booster effect's maintanance needs the instance to be activated
        isCollisionBehaviorCoroutineRunning=false;
        yield return null;
    }
}