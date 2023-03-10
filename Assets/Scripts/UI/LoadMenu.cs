using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LoadMenu : MonoBehaviour
{
    private GameManager _gameManager;

    [SerializeField]
    private GameObject _loadMenuUI;

    // Start is called before the first frame update
    void Awake()
    {
        _loadMenuUI.SetActive(false);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        _loadMenuUI.SetActive(true);
        _gameManager.TimeManager.SetTimeScale(0);
    }

    public void Close()
    {
        _loadMenuUI.SetActive(false);
        _gameManager.TimeManager.SetTimeScale(1);
    }
}
