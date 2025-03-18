using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision_river : Collision_normalObs
{   

    public GameObject player;
    public override IEnumerator CollisionBehavior(){
        if(playerstate.BoosterState=="Invincibility"){
            transform.position= new Vector3(30f,-2f,2f); //move it away from camera and look like "removed" If I put it back to the pool directly, the upcoming generation wil have giltch
            isCollisionBehaviorCoroutineRunning=false;
        }
        else{
            if(Input.GetMouseButton(0)){                    // if the left button is not released
                GameOverPanel.SetActive(true);             //GameOver panel will fade in in 0.55 s
                instructionPanel.SetActive(false);
                MenueButton.SetActive(false);
                Renderer renderer=GetComponent<Renderer>();
                Color originColor = renderer.material.color;
                renderer.material.color=new Color(0f, 0.8f, 1f, 1f);
                yield return new WaitForSeconds(0.2f);
                renderer.material.color=originColor;
                yield return new WaitForSeconds(0.2f);
                renderer.material.color=new Color(0f, 0.8f, 1f, 1f);
                yield return new WaitForSeconds(0.2f);
                renderer.material.color=originColor;
                isCollisionBehaviorCoroutineRunning=false;
                Time.timeScale=0;                               //stop everything when collision happend
            }
            else{
                Renderer playerRenderer=player.GetComponent<Renderer>();
                Color originColor=playerRenderer.material.color;            //blinking means the ball is jumping
                playerRenderer.material.color=new Color (0f, 0.8f, 1f, 1f);
                yield return new WaitForSeconds(0.2f);
                playerRenderer.material.color=originColor;
                yield return new WaitForSeconds(0.2f);
                playerRenderer.material.color=new Color (0f, 0.8f, 1f, 1f);
                yield return new WaitForSeconds(0.2f);
                playerRenderer.material.color=originColor;
            }
        }
    }
}
