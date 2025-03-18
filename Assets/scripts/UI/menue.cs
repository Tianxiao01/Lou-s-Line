using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class menue : MonoBehaviour
{
    public GameObject Menue;
    public GameObject instruction;
    public GameObject MenueButton;
    public void ShowMenue(){
        Menue.SetActive(true);
        instruction.SetActive(false);
        MenueButton.SetActive(false);
        Time.timeScale = 0f; // stop everthing
    }

    public void KeepGoing(){
        Menue.SetActive(false);
        instruction.SetActive(true);
        MenueButton.SetActive(true);
        Time.timeScale = 1f;
    }


}
