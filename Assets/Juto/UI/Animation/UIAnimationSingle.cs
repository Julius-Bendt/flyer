using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

namespace Juto.UI
{
    public class UIAnimationSingle : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
    {

        public UIAnimationGroupSingle onDown, onExit, OnEnter, onUp;

        public UIAnimationFramework effects;

        #region events
        public void OnPointerDown(PointerEventData eventData)
        {
            onDown.Invoke(effects);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnEnter.Invoke(effects);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onExit.Invoke(effects);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            onUp.Invoke(effects);
        }
        #endregion
    }

    [System.Serializable]
    public struct UIAnimationGroupSingle
    {
        public RectMoveAnimationSingle[] move;
        public RectScaleAnimationSingle[] scale;
        public RectRotateAnimationSingle[] rotate;
        public TextFadeAnimationSingle[] textFade;
        public ImageFadeAnimationSingle[] imageFade;
        public ImageFillAnimationSingle[] imageFill;
        public CanvasGroupFadeAnimationSingle[] cg;


        public void Invoke(UIAnimationFramework effects)
        {
            foreach (RectMoveAnimationSingle m in move)
            {
                effects.Move(m.rect, m.point, m.time);
            }

            foreach (RectScaleAnimationSingle s in scale)
            {
                effects.Scale(s.rect, s.point, s.time);
            }

            foreach (RectRotateAnimationSingle a in rotate)
            {
                effects.Rotate(a.rect, Quaternion.Euler(a.rotation), a.time,a.delay);
            }

            foreach (TextFadeAnimationSingle a in textFade)
            {
                effects.Fade(a.text, a.color, a.time, a.delay);
            }

            foreach (ImageFadeAnimationSingle a in imageFade)
            {
                effects.Fade(a.image, a.color, a.time, a.delay);
            }

            foreach (ImageFillAnimationSingle a in imageFill)
            {
                effects.Fill(a.image, a.value, a.time, a.delay);
            }

            foreach (CanvasGroupFadeAnimationSingle a in cg)
            {
                effects.Fade(a.cg, a.value, a.time, a.delay);
            }


        }
    }

    #region holder classes
    //Rect
    [System.Serializable]
    public struct RectMoveAnimationSingle
    {
        public RectTransform rect;
        public Vector3 point;
        public float time, delay;
    }

    [System.Serializable]
    public class RectScaleAnimationSingle
    {
        public RectTransform rect;
        public Vector3 point = Vector3.one;
        public float time, delay;
    }

    [System.Serializable]
    public class RectRotateAnimationSingle
    {
        public RectTransform rect;
        public Vector3 rotation;
        public float time, delay;
    }

    [System.Serializable]
    public class TextFadeAnimationSingle
    {
        public TextMeshProUGUI text;
        public Color color;
        public float time, delay;
    }

    //Image
    [System.Serializable]
    public class ImageFadeAnimationSingle
    {
        public Image image;
        public Color color = Color.white;
        public float time, delay;
    }

    [System.Serializable]
    public class ImageFillAnimationSingle
    {
        public Image image;
        public float value;
        public float time, delay;
    }

    //Canvas group
    [System.Serializable]
    public class CanvasGroupFadeAnimationSingle
    {
        public CanvasGroup cg;
        public float value;
        public float time, delay;
    }



    #endregion




}
