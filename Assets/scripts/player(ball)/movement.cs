using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public int speed;
    private Vector3 newposition = new Vector3 (0f,0-1.19f,0f);

    public RectTransform panel;
    

    // Update is called once per frame
    void Update()
    {   
        if (Input.GetMouseButton(0)){                                                         //if the left button is not clicked do nothing for optimization purpose
            Vector2 mouseScreenPosition = Input.mousePosition;
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition); // transform the screen coordinate to world coordinate
            if (RectTransformUtility.RectangleContainsScreenPoint(panel,mouseScreenPosition)){
                newposition.x=mouseWorldPosition.x;
                transform.position=newposition;
                //Debug.Log(mouseWorldPosition);
            }
        }
        
    }
}
