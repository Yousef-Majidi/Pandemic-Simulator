using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScripts : MonoBehaviour{

    public Animator transition;

    public void Close(){
        Application.Quit();
        Debug.Log("Game is exiting");
    }

    IEnumerator wait(string name){
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        changeScene(name);
    }

    public void changeScene(string name){
        SceneManager.LoadScene(name);
        Debug.Log("Scene is changing to " + name);
    }
}

