using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract  class BoosterBase : MonoBehaviour
{
    public Booster_state playerState;
    protected collision_behavior CollisionChecker;
    public string BoosterName;
    public float duration;

    protected bool CollisionHappens;
    protected bool CoroutineisOn=false;

    void Applystate(){
        playerState.BoosterState=BoosterName;
        
    }

    void ReturnToOriginState(){
        playerState.BoosterState="None";
        Debug.Log("return");
    }

    void Start()
    {   
        CollisionChecker = GetComponent<collision_behavior>(); 
        
    }

    IEnumerator AffectPlayer(){
        Applystate();
        //Debug.Log("2");
        yield return new WaitForSeconds(5.0f);
        ReturnToOriginState();
        CoroutineisOn=false;
        
    }

    void Update()
    {
        CollisionHappens=CollisionChecker.isCollided;
        Debug.Log(CollisionHappens);
        if(CollisionHappens && !CoroutineisOn){
            CoroutineisOn=true;
            StartCoroutine(AffectPlayer());
        }
    } 
}
