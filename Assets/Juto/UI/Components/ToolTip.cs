using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Juto.UI
{
    public class ToolTip : MonoBehaviour
    {


        const float FADETIME = 0.15f;
        const float SCALETIME = 0.1f;

        TextMeshProUGUI textComponent;
        Image start, fill;

        RectTransform startRect;
        RectTransform fillRect;
        RectTransform textRect;

        private UIAnimationFramework animations;


        public void Init(string text, int textSize, TextAlignmentOptions alignment = TextAlignmentOptions.MidlineJustified)
        {

            if (animations == null)
                animations = FindObjectOfType<UIAnimationFramework>();

            if (animations == null)
                Debug.LogError("Couldn't find UI Animations");

            textComponent = new GameObject("text").AddComponent<TextMeshProUGUI>();
            start = new GameObject("Start").AddComponent<Image>();
            fill = new GameObject("Fill").AddComponent<Image>();

            textComponent.color = Color.clear;
            start.color = Color.clear;
            fill.color = Color.clear;

            //Load start sprite
            Sprite toolTipStart = Resources.Load<Sprite>("tooltip_start");

            if (toolTipStart == null)
                Debug.LogError("Couldn't find tool tip start sprite!");

            startRect = start.GetComponent<RectTransform>();
            fillRect = fill.GetComponent<RectTransform>();
            textRect = textComponent.GetComponent<RectTransform>();




            start.transform.SetParent(transform);
            fill.transform.SetParent(transform);
            textComponent.transform.SetParent(fill.transform);

            //set everything to 0.
            startRect.anchoredPosition = Vector3.zero;

            start.sprite = toolTipStart; //assign the start sprite

            //Fix sizing with the 
            textComponent.text = text;
            textComponent.fontSize = textSize;


            float width = textComponent.preferredWidth * 1.1f;
            float height = textComponent.preferredHeight*1.1f;

            startRect.sizeDelta = new Vector2(height, height); //set height and width. (height in both vars, because the ratio is 1/1.)
            fillRect.sizeDelta = new Vector2(width, height);
            fillRect.anchoredPosition = new Vector2((height - width + (textComponent.preferredWidth * 0.1f)) * 0.9f, 0);


            textComponent.alignment = alignment; //assign the text aligment
            textComponent.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            textComponent.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            textRect.offsetMin = textRect.offsetMax = Vector2.zero;



            //Animations!
            animations.Fade(start, Color.white, FADETIME);
            animations.Fade(fill, Color.white, FADETIME);
            animations.Scale(startRect, Vector3.one, SCALETIME);
            animations.Scale(fillRect, Vector3.one, SCALETIME);
            animations.Fade(textComponent, Color.black, FADETIME);
            animations.Scale(textRect, Vector3.one, SCALETIME);




        }

        public void Close()
        {


            animations.Scale(textRect, Vector3.zero, SCALETIME);
            animations.Scale(startRect, Vector3.zero, SCALETIME);
            animations.Scale(fillRect, Vector3.zero, SCALETIME+0.01f,0, realClose);

        }

        private void realClose()
        {
            Destroy(gameObject);
        }
    }

}
