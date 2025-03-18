using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacle_movement : MonoBehaviour
{
    public GameManager GM;
    private Vector3 newPosition;
    public static float speed=4.0f;

    private string Obsname;

    void Start()
    {
        Obsname=gameObject.name;
    }

    void Update()
    {   


        Vector3 newPosition= transform.position;
        newPosition.y-=speed*Time.deltaTime;
        transform.position=newPosition;

        if(transform.position.y<-5){                                //if the obstacles reach (x,-5), it goes back to pool
            ObstaclePool[] allPools = FindObjectsOfType<ObstaclePool>(); //each type of obstacles will returen to its respecrtive pool
            //Debug.Log(name);
            if(Obsname=="obstacle(Clone)"){
                //Debug.Log(name);
                foreach (var pool in allPools) {
                    if(pool.gameObject.name == "ObstaclePool"){
                        pool.ReturnObj(this.gameObject);
                    }
                }

            }
            else if(Obsname=="river(Clone)"){
                foreach (var pool in allPools) {
                    if(pool.gameObject.name == "RIverPool"){
                        pool.ReturnObj(this.gameObject);
                    }
                }
            }
            
        }
    }
}
