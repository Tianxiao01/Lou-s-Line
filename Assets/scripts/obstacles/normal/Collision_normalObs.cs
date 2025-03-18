using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision_normalObs : MonoBehaviour
{
    public Transform playerTransform;
    public Booster_state playerstate;

    public ObstaclePool ObsPool;

    public GameObject GameOverPanel;
    public GameObject MenueButton;
    public GameObject instructionPanel;

    public bool isCollided=false; 
    protected bool isCollisionBehaviorCoroutineRunning = false; //we don't want multiple coroutine be created when collison happen

 
    void Start()
    {
        StartCoroutine(CheckCollision());
    }

    void OnEnable()                         // after the object is reactivated enable coroutine again
    {
        StartCoroutine(CheckCollision());
    }

    void OnDisable()
    {
        StopCoroutine(CheckCollision());
    }

    public virtual IEnumerator  CheckCollision(){ 
        Vector3 closestPt=transform.position;
        while (true){
            Vector3 playerPosition=playerTransform.position;
            Vector3 ObsPositon=transform.position;
            if(ObsPositon.x-1f>playerPosition.x){
                closestPt.x=ObsPositon.x-1f;
            }
            else if (ObsPositon.x+1f<playerPosition.x){
                closestPt.x=ObsPositon.x+1f;
            }
            else{
                closestPt.x=playerPosition.x;
            }

            if((ObsPositon.y-1.5f>playerPosition.y)){
                closestPt.y=ObsPositon.y-1.5f;
            }
            else if (ObsPositon.y+1.5f<playerPosition.y){
                closestPt.y=ObsPositon.y+1.5f;
            }
            else{
                closestPt.y=playerPosition.y;
            }



            if (closestPt.x<ObsPositon.x+1f && closestPt.x>ObsPositon.x-1f
            && closestPt.y<ObsPositon.y+1.5f && closestPt.y>ObsPositon.y-1.5f){
                isCollided=true;
                //Debug.Log("isCollided1");
            }
            else{
                float distance=CalculateDistance(closestPt,playerPosition);
                if(playerstate.BoosterState=="Small_Size"){
                    if(distance<0.25f){
                         isCollided=true;
                    //Debug.Log("isCollided2");
                    }
                    else{
                        isCollided=false;
                    }
                }
                else{
                    if(distance<0.5f){
                        isCollided=true;
                    //Debug.Log("isCollided2");
                    }
                    else{
                        isCollided=false;
                    }
                }
            }
            
            
        yield return null; // check collision every frame
        }
    }

    protected float CalculateDistance(Vector3 clsPt, Vector3 playerPt){
        float distance=100f;
        float XDiffer=clsPt.x-playerPt.x;
        float YDiffer=clsPt.y-playerPt.y;
        distance=Mathf.Sqrt(XDiffer*XDiffer+YDiffer*YDiffer);
        return distance;
    }

    public virtual IEnumerator  CollisionBehavior(){
        
        if(playerstate.BoosterState=="Invincibility"){
            transform.position= new Vector3(-30f,-2f,2f); //move it away from camera and look like "removed" If I put it back to the pool directly, the upcoming generation wil have giltch
            isCollisionBehaviorCoroutineRunning=false;
        }
        else{
            GameOverPanel.SetActive(true);             //GameOver panel will fade in in 0.55 s
            instructionPanel.SetActive(false);
            MenueButton.SetActive(false);
            Renderer renderer=GetComponent<Renderer>();
            Color originColor = renderer.material.color;
            renderer.material.color=new Color(1f, 0.8f, 0f, 1f);
            yield return new WaitForSeconds(0.2f);
            renderer.material.color=originColor;
            yield return new WaitForSeconds(0.2f);
            renderer.material.color=new Color(1f, 0.8f, 0f, 1f);
            yield return new WaitForSeconds(0.2f);
            renderer.material.color=originColor;
            isCollisionBehaviorCoroutineRunning=false;
            Time.timeScale=0;                               //stop everything when collision happend
        }
    }

    void Update()
    {
        if (isCollided && !isCollisionBehaviorCoroutineRunning){
            isCollisionBehaviorCoroutineRunning=true;
            StartCoroutine(CollisionBehavior());
        }
    }
}
