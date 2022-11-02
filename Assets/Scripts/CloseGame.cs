using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseGame : MonoBehaviour{
    public void Close()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }
}

