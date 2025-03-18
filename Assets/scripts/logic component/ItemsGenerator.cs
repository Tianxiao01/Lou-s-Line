using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;

using UnityEngine;
using UnityEngine.Rendering;

public class ItemsGenerator : MonoBehaviour
{   
    public GameObject jumpingPoint;         // this one is prefab
    private GameObject jumpPoint;            // instance
    private int curvedPathIndex;            // used in Pattern 2 
    public ObstaclePool ObsPool;
    public ObstaclePool RivPool;

    public ObstaclePool boosterPool;
    public GameManager GM;

    private bool IsTurned=false;                // each time the essential grid is called plus one to ensure no consecutive turns are made
    private int counter=0;                      // each time new rows are generated counter plus 1
    private int boostergenerationRecordr;       // no consecutive booster will occur
    private int boostergenerationCounter;

    public int[,] Essential_Grid;               // for the rest of map generation only top 2 rows are essential



    private Vector3 Init_IndexToCoordinate (int row,int column){  //when first build the scene, convert index number to exact positon on the screen
        Vector3 position=new Vector3 (0f,0f,2f);
        position.x=(2*row-10f)+1f;                              //transform the relative origin to (-10,10)
        position.y=(-3*column+10f)-1.5f;
        return position;
    }


    public Vector3 IndexToCoordinate (int row,int column){  //convert index number to exact positon on the screen
        Vector3 position=new Vector3 (0f,0f,2f);            // make z 2 f is to make the height of it differ from the player
        position.x=(2*row-10f)+1f;                              //transform the relative origin to (-10,11.5)
        position.y=(-3*column+11.5f)-1.5f;
        return position;
    }

    void InitEssentialGrid(){
        Essential_Grid = new  int[2, 10];  
        for (int i = 0; i < 2; i++) {
            for (int j = 0; j < 10; j++) {
                Essential_Grid[i,j]=GM.Start_Grid_map[i,j];
            }
        }
    }


    void Start()
    {   

        jumpPoint = Instantiate(jumpingPoint);              // init jumppoint（the dark block right before river) but set it to deactivated at first
        jumpPoint.SetActive(false);

        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 10; j++) {
                if (GM.Start_Grid_map[i,j]==0){
                    GameObject Obstacle=ObsPool.GetObj();
                    Obstacle.transform.position=Init_IndexToCoordinate(j,i);
                }
            }
        }

        InitEssentialGrid();
        InvokeRepeating("EssentialGridRegenerate_pattern1", 0.36f, 0.75f);// call pattern1 generation at 0.36 s and interval is 0.75 s
    }                                                                                   

        void GenerateNewRow(){                              //will be triggered after new Essential_Grid_pattern is made
        for (int i=0;i<10;i++){
            if (Essential_Grid[0,i]==0){                    // if the state of the block is 0 put a nomral obstacle block there
                GameObject obstacle= ObsPool.GetObj();
                obstacle.transform.position= IndexToCoordinate(i,0);
            }
            else if(Essential_Grid[0,i]==3){                // if the state of the block is 3 put a river block there
                GameObject riv= RivPool.GetObj();
                riv.transform.position= IndexToCoordinate(i,0); 
                Vector3 newZposition=riv.transform.position;    //because transform.positon is read only we have to using a new Vector3
                newZposition.z=1f;
                riv.transform.position=newZposition;
            }
            
            else if((Essential_Grid[0,i]==2 ||Essential_Grid[0,i]==4)&& boostergenerationCounter>=boostergenerationRecordr ){                
                                                                    // 10% chance there will be a booster on path
                int randomnumber=UnityEngine.Random.Range(1, 101);  // and gap between booster is at least 10 rows
                if(randomnumber<1000){
                    boostergenerationCounter=counter-10;
                    boostergenerationRecordr=counter;
                    GameObject booster= boosterPool.GetObj();
                    booster.transform.position= IndexToCoordinate(i,0);
                }
            }
            
            else if(Essential_Grid[0,i]==5){                // if the state of the block is 5 put a deep color path block there
                jumpPoint.SetActive(true);
                jumpPoint.transform.position=IndexToCoordinate(i,0);
            }
        }
        boostergenerationCounter++;
    }



    void Turn(bool isRight, int distance, int rootIndex){
        if (distance==0){                       //straight
            Essential_Grid[0,rootIndex]=2;
        }
        else{
            if(isRight){
                //Debug.Log("right turn");
                for (int i=rootIndex; i<rootIndex+distance;i++){
                    Essential_Grid[0,i]=1;
                    if (i==rootIndex+distance-1){
                        Essential_Grid[0,i+1]=2;
                    }
                }    
            } 
            else{
                //Debug.Log("left turn");
                for (int i=rootIndex; i>rootIndex-distance;i--){
                    Essential_Grid[0,i]=1;
                    if (i==rootIndex-distance+1){
                        Essential_Grid[0,i-1]=2;
                    }
                } 
            }
            IsTurned=true;
        }
    }

    public void EssentialGridRegenerate_pattern1(){  //first pattern to generate obstacles

        int randomNumebr=UnityEngine.Random.Range(1, 101);
        counter++;
        int indexNode=0;                                      

        
        for (int j = 0; j < 10; j++) {              //the previous top row has moved to second row
            Essential_Grid[1,j]=Essential_Grid[0,j];
        }
        
        for (int j = 0; j < 10; j++) {              //set new top row all to obstacle 
                Essential_Grid[0,j]=0;
        }


        for (int j = 0; j < 10; j++) {
            if ((Essential_Grid[1,j]==2 && IsTurned) || (Essential_Grid[1,j]==2 && counter==25) ){   
                                                            //if the block is a node (represented by 2) and there was a turn on last row, 
                Essential_Grid[0,j]=2;                      //the new top row will not perform turn behavior
                indexNode=j;                                // if it's the row before river it must be a straight path
                IsTurned=false;
            }              
            else if(Essential_Grid[1,j]==2){         // check which block is the node 
                indexNode=j;
                if (randomNumebr>80 && j<8){         //20% right turn will occurs(if turnable)
                    Turn(true,1,j);
                }
                else if(randomNumebr>60 && j>1){    //20% left turn will occurs(if turnable)
                    Turn(false,1,j);
                } 
                else if (randomNumebr>45 && j>3){   //15% a 3 bolcks long left term occurs(if turnable)
                    Turn(false,3,j);
                }
                else if (randomNumebr>30 && j<6){   //15% a 3 bolcks long right term occurs(if turnable)
                    Turn(true,3,j);
                }
                else{
                    Essential_Grid[0,j]=2;          // (greater than) 30% it's a straight path
                }
                break;   
            }
        }


        if (counter==25){
            Essential_Grid[0,indexNode]=5;         // if the block is the one just before the river the generator will make it deep color
        }

        GenerateNewRow();

        if (counter>=25){                           // after 25 rows are generated using pattern1 change to pattern 2 and
            InvokeRepeating("EssentialGridRegenerate_pattern2",0.75f,0.75f);
            CancelInvoke("EssentialGridRegenerate_pattern1");
            counter=0;                              // reset coutner
        }       
             
    }

    public void EssentialGridRegenerate_pattern2(){  //second pattern to generate obstacles
        //Debug.Log("patter2 is on");

        int randomNumebr=UnityEngine.Random.Range(1, 101);
        counter++;
        

        
        for (int j = 0; j < 10; j++) {                  // move the previous top row to second row
            Essential_Grid[1,j]=Essential_Grid[0,j];
        }
        
        if (counter==1){                                // if it is the first row generated under pattern2 
            for (int j = 0; j < 10; j++) {              //set new top row all to river
                Essential_Grid[0,j]=3;
            }
        }
        else if(counter==2){
            for (int j = 0; j < 10; j++) {              //set new top row all to path
                Essential_Grid[0,j]=1;
            }
        }
        else if(counter==3){
            for (int j = 0; j < 10; j++) {              //set new top row all to path then choose 2 nodes one for straight path (8)
                Essential_Grid[0,j]=0;                  //one for curve path but well-rewoard (2 for node 1 for normal path like pattern 1)
            }                                           // eight pronouce more like straight
            if(randomNumebr<=50){                       // 50% the straight path will be the left one 50 be the right one
                curvedPathIndex=2;
                Essential_Grid[0,2]=2;
                Essential_Grid[0,7]=8;
            }
            else{
                curvedPathIndex=7;
                Essential_Grid[0,7]=2;
                Essential_Grid[0,2]=8;
            }
        }
        else if(counter<=21){
            for (int j = 0; j < 10; j++) {              //set new top row all to obstacle 
                Essential_Grid[0,j]=0;
            }

            for (int j = 0; j < 9; j++) {                       // no need to check index 9 because no path will reach there in pattern 2
                if (Essential_Grid[1,j]==2 && IsTurned ||(Essential_Grid[1,j]==2 && counter==21) ){   
                                                                //if the block is a node (represented by 2) and there was a turn on last row, 
                    Essential_Grid[0,j]=2;                      //the new top row will not perform turn behavior
                    IsTurned=false;
                }              
                else if(Essential_Grid[1,j]==2){                // check which block is the node 

                    if (randomNumebr>80 && j<curvedPathIndex+1){         //20% right turn will occurs (if turnable)
                        Turn(true,1,j);
                    }
                    else if(randomNumebr>60 && j>curvedPathIndex-1){    //20% left turn will occurs(if turnable)
                        Turn(false,1,j);
                    } 
                    else if (randomNumebr>40 && j>curvedPathIndex){   //20% a 2 bolcks long left turn occurs(if turnable)
                        Turn(false,2,j);
                    }
                    else if (randomNumebr>20 && j<curvedPathIndex){   //20% a 2 bolcks long right turn occurs(if turnable)
                        Turn(true,2,j);
                    }
                    else{
                        Essential_Grid[0,j]=2;                          // (greater than)20% it's a straight path
                    }
                }

                if (Essential_Grid[1,j]==8){
                    Essential_Grid[0,j]=8;
                }  
            }
        }
        else if(counter==22){
            for (int j = 0; j < 10; j++) {              //set new top row all to obstacle 
                Essential_Grid[0,j]=0;
            }

            for(int i=0;i<9;i++){                       // merge the curved path with the straight one
                if (Essential_Grid[1,i]==2){
                    if(curvedPathIndex==2){
                        for(int j=i;j<7;j++){
                        Essential_Grid[0,j]=1;
                        }
                        Essential_Grid[0,7]=2;
                    }
                    else{
                        for(int j=i;j>2;j--){
                        Essential_Grid[0,j]=1;
                        }
                        Essential_Grid[0,2]=2;
                    }
                }         
            }
        }
        else{
            for (int j = 0; j < 10; j++) {              //set new top row all to obstacle 
                Essential_Grid[0,j]=0;
            }
            if(curvedPathIndex==2){
                Essential_Grid[0,7]=2;
            }
            else{
                Essential_Grid[0,2]=2;
            }
            
        }

         if(randomNumebr<=15){                        //（more than)15% extra chance booster spawns on curved path
            for(int i=1;i<9;i++){
                if (Essential_Grid[0,i]==1){
                    Essential_Grid[0,i]=4;
                    break;
                }
            } 
        }

        GenerateNewRow();

        if (counter>=25){                           // after 25 rows are generated change back to pattern 1
            InvokeRepeating("EssentialGridRegenerate_pattern1",0.75f,0.75f);
            CancelInvoke("EssentialGridRegenerate_pattern2");
            counter=0;                              // reset coutner
        }   
                                    
    }

    

    void PrintMap2() {
        string mapString = "map statuss:\n";
        for (int i = 0; i < 2; i++) {
            for (int j = 0; j < 10; j++) {
                mapString += Essential_Grid[i, j] + " ";
            }
            mapString += "\n";
        }
        Debug.Log(mapString);
    }

    void Update()
    {
        //PrintMap2();
        //Debug.Log(ObsPool.CountElements());
    }
}
