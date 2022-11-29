using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScripts : MonoBehaviour{
    private int _width;
    private int _height;
    private string _resolution = "1920x1080";
    private int _quality = 5;
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

    public void ChangeScreenResolution(string resolution){
        int width = 0;
        int height = 0;
        string[] res = resolution.Split('x');
        width = int.Parse(res[0]);
        height = int.Parse(res[1]);
        _width = width;
        _height = height;

    }

    public void ChangeQuality(int qualityIndex){
        _quality = qualityIndex;
    }

    public void SaveOptions(){
        Screen.SetResolution(_width, _height, true);
        QualitySettings.SetQualityLevel(_quality);
        Debug.Log("Options are saved");
    }

    public void setToDefault(){
        ChangeScreenResolution(_resolution);
        ChangeQuality(5);
        Debug.Log("Options are set to default");
    }

    IEnumerator LoadLevel(string name){
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(name);
        Debug.Log("Scene is changing to " + name);
    }

}

