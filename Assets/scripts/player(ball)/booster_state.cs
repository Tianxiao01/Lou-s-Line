using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster_state : MonoBehaviour
{   
    public string BoosterState="None";  //Invincibility None Small_Size


    void Update()
    {   
        if(BoosterState=="Small_Size"){
            transform.localScale=new Vector3(0.5f,0.5f,0.5f);
        }
        else if(BoosterState=="Invincibility") {
            transform.localScale=new Vector3(1f,1.5f,1f);
        }
        else{
            transform.localScale=new Vector3(1f,1f,1f);
        }
        Debug.Log(BoosterState);
    }
}
