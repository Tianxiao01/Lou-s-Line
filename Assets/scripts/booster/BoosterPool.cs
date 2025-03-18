using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterPool : ObstaclePool
{
   public GameObject targetObject2; 



    public override void InitPool(){
            GameObject obj = Instantiate(targetObject);
            obj.SetActive(false);  
            pool.Enqueue(obj);

            GameObject obj2 = Instantiate(targetObject2);
            obj2.SetActive(false);  
            pool.Enqueue(obj2);
        }


   public override GameObject GetObj(){
        GameObject obj=pool.Dequeue();
        int randomNumebr=UnityEngine.Random.Range(1,101);  
        if(randomNumebr<=50){                                //there will be 50% chance getObj get the next obj to shuffle the order in queue generatin some random feature
            this.ReturnObj(obj);
            obj=pool.Dequeue();
        }                
        obj.SetActive(true);
        return obj;
    }  
}
