using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPopUp : MonoBehaviour
{

    private bool isPopUp = false;

    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OnMouseDown();
    }

     void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider.gameObject.CompareTag("NPC"))
                {
                    NpcPopUp(hit.collider.gameObject);
                    isPopUp = true;
                }
                else 
                {
                    if(isPopUp)
                    {
                        GameObject canvasTemp = GameObject.Find("CanvasPop");
                        if (canvasTemp != null)
                        {
                            Destroy(canvasTemp);
                            isPopUp = false;
                        }
                    }
                }
            }
        }
    }

    void NpcPopUp(GameObject obj)
    {
        // see if there is already a canvas
        GameObject canvasTemp = GameObject.Find("CanvasPop");
        if (canvasTemp != null)
        {
            Destroy(canvasTemp);
        }

        // create canvas
        GameObject canvas = new GameObject("CanvasPop", typeof(RectTransform));
        canvas.AddComponent<Canvas>();
        canvas.AddComponent<CanvasScaler>();
        canvas.AddComponent<GraphicRaycaster>();
        

        // create panel
        GameObject panel = new GameObject("Panel", typeof(RectTransform));
        panel.transform.SetParent(canvas.transform, false);
        panel.AddComponent<Image>();

        //create button
        GameObject exitButton = new GameObject("Button", typeof(RectTransform));
        exitButton.transform.SetParent(panel.transform, false);
        exitButton.AddComponent<Button>();
        exitButton.AddComponent<Image>();
        exitButton.GetComponent<Button>().onClick.AddListener(() => Destroy(canvas));

        // add text from TextMeshPro to button 
        GameObject buttonText = new GameObject("Text", typeof(RectTransform));
        buttonText.transform.SetParent(exitButton.transform, false);
        buttonText.AddComponent<TextMeshProUGUI>();

        //create text
        GameObject text = new GameObject("Text", typeof(RectTransform));
        text.transform.SetParent(panel.transform, false);
        text.AddComponent<TextMeshProUGUI>();

        // set canvas to only take up the bottom left corner of the display 
        canvas.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        canvas.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
        canvas.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
        canvas.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

        // set panel properties to be only top left corner of canvas
        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(900, 900);
        panel.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.5f);
        panel.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0.5f);
        panel.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
        panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        panel.GetComponent<Image>().color = 0.75f * Color.white;


        exitButton.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
        exitButton.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
        exitButton.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
        exitButton.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
        exitButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -200);
        exitButton.GetComponent<RectTransform>().SetAsLastSibling();
        exitButton.GetComponent<Image>().color = Color.red;


        //set button text properties
        buttonText.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
        buttonText.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        buttonText.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
        buttonText.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
        buttonText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        buttonText.GetComponent<TextMeshProUGUI>().text = "X";
        buttonText.GetComponent<TextMeshProUGUI>().color = Color.black;
        buttonText.GetComponent<TextMeshProUGUI>().fontSize = 85;
        buttonText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;

        // set text properties and place in center of panel
        text.GetComponent<RectTransform>().sizeDelta = new Vector2(900, 900);
        text.GetComponent<TextMeshProUGUI>().text = "Name: " + obj.name + "\nHealth: " + obj.GetComponent<NPC>().Health.ToString("F2") + "\nStamina: " + obj.GetComponent<NPC>().Stamina.ToString("F2") + "\nInfected: " + obj.GetComponent<NPC>().IsInfected;
        text.GetComponent<TextMeshProUGUI>().color = Color.black;
        text.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
        text.GetComponent<TextMeshProUGUI>().fontSize = 75;
        text.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        text.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
        text.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
        text.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    }
}
