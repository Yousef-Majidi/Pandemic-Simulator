using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject OptionsMenuUI;

    public void open()
    {
        OptionsMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void close()
    {
        OptionsMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    void Awake()
    {
        OptionsMenuUI.SetActive(false);
    }



}
