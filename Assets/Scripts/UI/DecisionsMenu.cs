using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DecisionsMenu : MonoBehaviour
{
    LinkedList<Decisions> decisions = new LinkedList<Decisions>();
  
    void Awake()
    {
        //create a decision 
        Decisions decision1 = Decisions.CreateInstance<Decisions>();
        decision1.description = "This is a decision";
        decision1.title = "Decision";
        decision1.id = 0;
        decision1.healthEffect = 0;
        decision1.virusEffect = 0;
        decision1.happyEffect = 0;
        decision1.isActive = false;

        Decisions decision2 = Decisions.CreateInstance<Decisions>();
        decision2.description = "This is a decision";
        decision2.title = "Poop1";

        Decisions decision3 = Decisions.CreateInstance<Decisions>();
        decision3.description = "This is a decision";
        decision3.title = "Poop2";
        decision3.isActive = true;

        Decisions decision4 = Decisions.CreateInstance<Decisions>();
        decision4.description = "This is a decision";
        decision4.title = "Poop3";

        Decisions decision5 = Decisions.CreateInstance<Decisions>();
        decision5.description = "This is a decision";
        decision5.title = "Poop4";

        Decisions decision6 = Decisions.CreateInstance<Decisions>();
        decision6.description = "This is a decision";
        decision6.title = "Poop5";



        gameObject.SetActive(false);
        decisions.AddLast(decision1);
        decisions.AddLast(decision2);
        decisions.AddLast(decision3);
        decisions.AddLast(decision4);
        decisions.AddLast(decision5);
        decisions.AddLast(decision6);


    }

    public void close()
    {
        gameObject.SetActive(false);
    }

    public void ShowMenu()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            CreateUI();
        }
    }

    void PopUpDecision(Decisions decision){
        decision.isActive = true;

        //create canvas
        GameObject canvas = new GameObject("DecisionCanvas", typeof(RectTransform));
        canvas.AddComponent<Canvas>();
        canvas.AddComponent<CanvasScaler>();
        canvas.AddComponent<GraphicRaycaster>();
        canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        canvas.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        canvas.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 1000);
        canvas.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

        //create panel
        GameObject panel = new GameObject("Panel", typeof(RectTransform));
        panel.transform.SetParent(canvas.transform, false);
        panel.AddComponent<Image>();
        
        //create title
        GameObject title = new GameObject("Title", typeof(RectTransform));
        title.transform.SetParent(panel.transform, false);
        title.AddComponent<TextMeshProUGUI>();
        title.GetComponent<TextMeshProUGUI>().text = decision.title;
        title.GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 0, 255);
        title.GetComponent<TextMeshProUGUI>().fontSize = 80;

        //create description
        GameObject description = new GameObject("Description", typeof(RectTransform));
        description.transform.SetParent(panel.transform, false);
        description.AddComponent<TextMeshProUGUI>();
        description.GetComponent<TextMeshProUGUI>().text = decision.description;




    } 

    void CreateUI()
    {
        GameObject parent = GameObject.Find("Content");
        int buffer = 50;
        foreach (Decisions decision in decisions)
        {
            GameObject panel = new GameObject("Panel", typeof(RectTransform));
            panel.transform.SetParent(parent.transform, false);
            panel.AddComponent<Image>();

            GameObject title = new GameObject("Title", typeof(RectTransform));
            title.transform.SetParent(panel.transform, false);
            title.AddComponent<TextMeshProUGUI>();
            title.GetComponent<TextMeshProUGUI>().text = decision.title;
            title.GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 0, 255);
            title.GetComponent<TextMeshProUGUI>().fontSize = 80;


            GameObject description = new GameObject("Description", typeof(RectTransform));
            description.transform.SetParent(panel.transform, false);
            description.AddComponent<TextMeshProUGUI>();
            description.GetComponent<TextMeshProUGUI>().text = decision.description;

            GameObject button = new GameObject("Button", typeof(RectTransform));
            button.transform.SetParent(panel.transform, false);
            button.AddComponent<Button>();
            button.AddComponent<Image>();
            button.GetComponent<Button>().onClick.AddListener(() => PopUpDecision(decision));


            GameObject buttonText = new GameObject("ButtonText", typeof(RectTransform));
            buttonText.transform.SetParent(button.transform, false);
            buttonText.AddComponent<TextMeshProUGUI>();
            buttonText.GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 0, 255);

            panel.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
            panel.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
            panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            panel.GetComponent<RectTransform>().sizeDelta = new Vector2(parent.GetComponent<RectTransform>().rect.width - 100, 150);
            panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, panel.GetComponent<RectTransform>().anchoredPosition.y - buffer);
            buffer += 200;

            title.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.5f);
            title.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0.5f);
            title.GetComponent<RectTransform>().anchoredPosition = new Vector2(500, 0);
            title.GetComponent<RectTransform>().sizeDelta = new Vector2(panel.GetComponent<RectTransform>().rect.width * 0.3f, 50);

            if (decision.isActive)
            {
                button.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                buttonText.GetComponent<TextMeshProUGUI>().text = "Disenact";
                buttonText.GetComponent<TextMeshProUGUI>().fontSize = 50;
                buttonText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -25);

            }
            else
            {
                button.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                buttonText.GetComponent<TextMeshProUGUI>().text = "Enact";
                buttonText.GetComponent<TextMeshProUGUI>().fontSize = 60;
                buttonText.GetComponent<RectTransform>().anchoredPosition = new Vector2(20, -15);
            }

            button.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0.5f);
            button.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0.5f);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(-150, 0);
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);

            buttonText.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            buttonText.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            buttonText.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);





        }
    }


}
