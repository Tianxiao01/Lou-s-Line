using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public int[,] Start_Grid_map;  

    public TextMeshProUGUI  startinstruction;


    public bool isStarted=false;
    public GameObject StartingPt; // used to record how "far" the player has travelled
    public GameObject player;
    public TextMeshProUGUI scoreText;
   
    void InitMap(){
        Start_Grid_map = new  int[5, 10];  //2-dimension array to represent pattern of starting grid in 5*10 maps. 0 means the grid will be obstacle, 1 means it's a part of the path
        for (int i = 0; i < 5; i++) {       //2 means it's a crucial node of the path
            for (int j = 0; j < 10; j++) {
                if (i+j>=4 && j-i<=5 && i<4){
                    Start_Grid_map[i,j]=1;
                }
                else{

                }
            }
        }
        if (UnityEngine.Random.Range(1, 101)<50){
            Start_Grid_map[0,4]=2;
        }
        else{
            Start_Grid_map[0,5]=2;
        }
    }

    
     void PrintMap() {
        string mapString = "map statuss:\n";
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 10; j++) {
                mapString += Start_Grid_map[i, j] + " ";
            }
            mapString += "\n";
        }
        Debug.Log(mapString);
    }
    
    
    void Start()
    {
        Time.timeScale=0;
        InitMap();
        StartCoroutine(checkStarted()); // wait for left button is clicked to start game
    }

    IEnumerator checkStarted(){
        while(!isStarted){
            if (Input.GetMouseButtonDown(0)){
            Time.timeScale=1;
            isStarted=true;
            startinstruction.gameObject.SetActive(false);
            }
            yield return null;
        }
    }

    void Update()
    {   
        float score =2*(player.transform.position.y-StartingPt.transform.position.y);                    //per score is gained in 0.5 s
        scoreText.text=score.ToString("0");
        //PrintMap();
    }

    public void Restart(){
        SceneManager.LoadScene("main scene");
    }
}
