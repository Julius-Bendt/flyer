using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace Juto.UI
{
    // Button that's meant to work with mouse or touch-based devices.
    [AddComponentMenu("UI/Flat UI/Flat Button", 30)]
    public class FlatButton : Selectable, IPointerClickHandler, ISubmitHandler
    {
        [Serializable]
        public class ButtonClickedEvent : UnityEvent { }


        [Header("Text options")]
        [FormerlySerializedAs("Text ")]
        [SerializeField]
        private string buttonText = "Button";

        [FormerlySerializedAs("Text font size")]
        [SerializeField]
        private float fontSize = 14;

        [FormerlySerializedAs("Text color")]
        [SerializeField]
        private Color textColor;

        [FormerlySerializedAs("Text")]
        [SerializeField]
        private TextMeshProUGUI text;

        [Header("Icon options")]
        [FormerlySerializedAs("Icon Color")]
        [SerializeField]
        private Color iconColor;
        [FormerlySerializedAs("Icon")]
        [SerializeField]
        private Sprite icon;
        [FormerlySerializedAs("Icon ")]
        [SerializeField]
        private Image iconImage;

        [Header("Shadow options")]
        [FormerlySerializedAs("Shadow color")]
        [SerializeField]
        private Color shadowColor;
        [FormerlySerializedAs("Shadow")]
        [SerializeField]
        private Graphic shadow;



        // Event delegates triggered on click.
        [Space(25)]
        [FormerlySerializedAs("onClick")]
        [SerializeField]
        private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

        protected FlatButton()
        { }

        public ButtonClickedEvent onClick
        {
            get { return m_OnClick; }
            set { m_OnClick = value; }
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            if(isActiveAndEnabled)
            {
                if(text != null)
                {
                    text.text = buttonText;
                    text.fontSize = fontSize;
                    text.color = textColor;
                }

                if (iconImage != null)
                {
                    iconImage.sprite = icon;
                    iconImage.color = (icon != null) ? iconColor : Color.clear;

                }

                Resize();

                if (shadow != null)
                {
                    shadow.color = shadowColor;
                }
            }
        }

        private void Resize()
        {
            //Update text size
            RectTransform textRect = text.GetComponent<RectTransform>();
            RectTransform iconRect = iconImage.GetComponent<RectTransform>();
            RectTransform rect = GetComponent<RectTransform>();

            float iconSize = (rect.sizeDelta.x < rect.sizeDelta.y) ? rect.sizeDelta.x : rect.sizeDelta.y;
            iconSize *= 0.9f;

            Debug.Log(iconSize);

            //Check for content to resize
            bool iconEnabled = false, textEnabled = false;

            if (iconImage != null && icon != null)
            {
                iconEnabled = true;
            }

            if (text != null && !string.IsNullOrEmpty(buttonText))
            {
                textEnabled = true;
            }

            //Resize if needed

            if (iconEnabled && !textEnabled)
            {
                iconRect.anchorMin = new Vector2(0.5f, 0.5f);
                iconRect.anchorMax = new Vector2(0.5f, 0.5f);
                iconRect.offsetMin = new Vector2(0f, 0f); // new Vector2(left, bottom); 
                iconRect.offsetMax = new Vector2(0f, 0f); // new Vector2(-right, -top);
            }
            else if (!iconEnabled && textEnabled)
            {
                //Text should be full
                textRect.anchorMin = Vector2.zero;
                textRect.anchorMax = Vector2.one;
                textRect.offsetMin = new Vector2(0f, 0f); // new Vector2(left, bottom); 
                textRect.offsetMax = new Vector2(0f, 0f); // new Vector2(-right, -top);

            }
            else
            {
                iconRect.anchorMin = new Vector2(0, 0.5f);
                iconRect.anchorMax = new Vector2(0, 0.5f);
                iconRect.offsetMin = new Vector2(iconSize / 2, 0f); // new Vector2(left, bottom); 
                textRect.offsetMin = new Vector2(iconSize, 0f); // new Vector2(left, bottom); 
            }
        }
#endif

        private void Press()
        {
            if (!IsActive() || !IsInteractable())
                return;


            if(GetComponent<Ripple>())
            {
                GetComponent<Ripple>().Show();
            }

            m_OnClick.Invoke();
        }

        // Trigger all registered callbacks.
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            Press();
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
            Debug.Log("OnSubmit");

            Press();

            // if we get set disabled during the press
            // don't run the coroutine.
            if (!IsActive() || !IsInteractable())
                return;

            DoStateTransition(SelectionState.Pressed, false);
            StartCoroutine(OnFinishSubmit());
        }

        private IEnumerator OnFinishSubmit()
        {
            var fadeTime = colors.fadeDuration;
            var elapsedTime = 0f;

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            DoStateTransition(currentSelectionState, false);
        }
    }
}