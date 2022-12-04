using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMenu : MonoBehaviour{
    public static bool SaveMenuIsOpen = false;
    public GameObject SaveMenuUI;
    
    //on start, set the save menu to be closed
    void Start(){
        SaveMenuUI.SetActive(false);
    }

    public void open(){
        SaveMenuUI.SetActive(true);
        Time.timeScale = 0f;
        SaveMenuIsOpen = true;
    }

    public void close(){
        SaveMenuUI.SetActive(false);
        Time.timeScale = 1f;
        SaveMenuIsOpen = false;
    }
}
