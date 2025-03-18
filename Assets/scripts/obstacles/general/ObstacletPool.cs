using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ObstaclePool : MonoBehaviour
{   
    public GameObject targetObject;
    public int initSize=45;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void InitPool(){
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

    public GameObject GetObj(){
        if (pool.Count>0){                              //if there is object in the pool
            string instanceName = gameObject.name;
            //Debug.Log(instanceName+" get called");
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
        string instanceName = gameObject.name;
        //Debug.Log(instanceName+" return called");
        obj.SetActive(false);
        pool.Enqueue(obj);
        //Debug.Log("Pool size after return: " + pool.Count);
    }

    
}
