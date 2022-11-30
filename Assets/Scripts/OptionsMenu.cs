using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour{
    public static bool OptionsMenuIsOpen = false;
    public GameObject OptionsMenuUI;
    // Start is called before the first frame update
    void Start(){
        OptionsMenuUI.SetActive(false);
    }

    public void open(){
        OptionsMenuUI.SetActive(true);
        Time.timeScale = 0f;
        OptionsMenuIsOpen = true;
    }

    public void close(){
        OptionsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        OptionsMenuIsOpen = false;
    }

}
