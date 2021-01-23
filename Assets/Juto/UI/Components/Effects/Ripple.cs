using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Juto.UI
{
    [AddComponentMenu("UI/Flat UI/Effects/Ripple")]
    public class Ripple : MonoBehaviour
    {

        public float duration, blend, scaleFactor = 5;
        public Color color;
        public bool moveToMiddle;

        [Space]
        public Transform parent;

        private RectTransform rect, rippleRect;

        private UIAnimationFramework animationFramework;

        

        public void Show()
        {

            Debug.Log("Ripple on " + name);

            if (rect == null) //Use rect to get the size.
                rect = GetComponent<RectTransform>();

            if (parent == null)
                parent = transform;

            //make the ripple
            GameObject g = (GameObject)Instantiate(Resources.Load("Ripple"),Input.mousePosition,Quaternion.identity,parent);
            rippleRect = g.GetComponent<RectTransform>();
            g.GetComponent<Image>().color = color;



            //Find the smallest size
            float size = (rect.sizeDelta.x < rect.sizeDelta.y) ? rect.sizeDelta.x : rect.sizeDelta.y;
            //Set the size
            rippleRect.sizeDelta = new Vector2(size,size) * 0.5f;

            //Aaaaaand animate
            if (animationFramework == null)
                animationFramework = FindObjectOfType<UIAnimationFramework>();

            if (moveToMiddle)
                animationFramework.Move(rippleRect, rect.anchoredPosition, duration, 0);

            animationFramework.Scale(rippleRect, Vector3.one * scaleFactor, duration);
            animationFramework.Fade(g.GetComponent<Image>(), Color.clear, duration * 0.75f, duration * 0.25f);

            Destroy(g, duration + 0.1f);

        }

    }

}
