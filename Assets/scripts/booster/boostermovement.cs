using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boostermovement : MonoBehaviour
{
    public GameManager GM;
    private Vector3 newPosition;
    public static float speed=4.0f;


    void Update()
    {   


        newPosition= transform.position;
        newPosition.y-=speed*Time.deltaTime;
        transform.position=newPosition;

        if(transform.position.y<-5){                                //if the booster reach (x,-5), it goes back to pool
            ObstaclePool[] allPools = FindObjectsOfType<ObstaclePool>(); 
            //Debug.Log(name);

            //Debug.Log(name);
            foreach (var pool in allPools) {
                if(pool.gameObject.name == "BoosterPool"){
                    pool.ReturnObj(this.gameObject);
                }
            }
        }
    }
}
