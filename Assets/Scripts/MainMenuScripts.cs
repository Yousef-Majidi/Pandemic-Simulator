using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScripts : MonoBehaviour{


    public Animator transition;
    void Start(){
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log("Active scene is " + scene.name);
    }

    public void Close(){
        Application.Quit();
        Debug.Log("Game is exiting");
    }

    public void ChangeSceneWTransition(string name){

        StartCoroutine(LoadLevel(name));
    }

    IEnumerator LoadLevel(string name){
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(name);
        Debug.Log("Scene is changing to " + name);
    }
}

