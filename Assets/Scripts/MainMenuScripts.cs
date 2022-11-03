using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScripts : MonoBehaviour{
    public void Close(){
        Application.Quit();
        Debug.Log("Game is exiting");
    }

    public void changeScene(string name){
        SceneManager.LoadScene(name);
        Debug.Log("Scene is changing to " + name);
    }
}

