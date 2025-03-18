using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class booster_state : MonoBehaviour
{
    public string BoosterState="Small_Size";  //Invincibility None Small_Size

    void Update()
    {
        if(BoosterState=="Small_Size"){
            transform.localScale=new Vector3(0.5f,0.5f,0.5f);
        }
    }
}
