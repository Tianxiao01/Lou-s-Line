using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ObstaclePool : MonoBehaviour
{   
    public GameObject targetObject;
    public int initSize=45;

    protected Queue<GameObject> pool = new Queue<GameObject>();

    public virtual void InitPool(){
            for (int i = 0; i < initSize; i++) {
                GameObject obj = Instantiate(targetObject);
                obj.SetActive(false);  
                pool.Enqueue(obj);
            }
        }


    // Start is called before the first frame update
    void Start()
    {
        // initialization of pool and objects within (Obstacles ro River Blocks in this case)
        InitPool();
    }

    public int CountElements(){
        return pool.Count;
    }

    public virtual GameObject GetObj(){
        if (pool.Count>0){                              //if there is object in the pool
            GameObject obj=pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else{
            GameObject obj=Instantiate(targetObject);   //create one if all objects are ran out
            return obj;
        }
    }   

    public void ReturnObj (GameObject obj){
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    
}
