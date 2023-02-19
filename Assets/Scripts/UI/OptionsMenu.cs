using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    private GameManager _gameManager;

    [SerializeField]
    private GameObject OptionsMenuUI;

    public void open()
    {
        OptionsMenuUI.SetActive(true);
        _gameManager.TimeManager.SetTimeScale(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (OptionsMenuUI.activeSelf)
                close();
    }

    public void close()
    {
        OptionsMenuUI.SetActive(false);
        _gameManager.TimeManager.SetTimeScale(1);
    }

    void Awake()
    {
        OptionsMenuUI.SetActive(false);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }



}
