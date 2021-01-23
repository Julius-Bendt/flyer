using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour {

    public TutorialData[] tutorialData;
    public TextMeshProUGUI buttonText, descText;
    public Image image;
    private int i;

    bool playOnNextPress;

    private void Start()
    {
        Next();
    }

    public void Next()
    {
        if(i+1 >= tutorialData.Length)
        {
            buttonText.text = "Start game";
            descText.text = "Ready to start playing?";

            if(playOnNextPress)
            {
                App.Instance.uiManager.SwitchUI(UIManager.UIType.game);
            }

            playOnNextPress = true;
        }
        else
        {
            TutorialData current = tutorialData[i];
            image.sprite = current.sprite;
            descText.text = current.desc;
        }
        i++;
    }

    [System.Serializable]
    public struct TutorialData
    {
        public string name,desc;
        public Sprite sprite;
    }
}
