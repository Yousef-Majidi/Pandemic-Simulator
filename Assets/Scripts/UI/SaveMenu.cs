using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _saveMenuUI;

    public void open()
    {
        _saveMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void close()
    {
        _saveMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    void Awake()
    {
        _saveMenuUI.SetActive(false);
    }


}
