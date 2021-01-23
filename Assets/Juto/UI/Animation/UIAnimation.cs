using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace Juto.UI
{
    [RequireComponent(typeof(UIAnimationFramework))]
    public class UIAnimation : MonoBehaviour
    {
        public bool closeAllWhenPlaying = true;

        [Header("Animation groups")]
        public AnimationGroup[] animations;


        public void Toggle(string name, int id)
        {
            FindAndAnimate(name, true, false,id);
        }

        public void Open(string name, int id)
        {
            FindAndAnimate(name, false, true, id);
        }

        public void Close(string name,int id)
        {
            FindAndAnimate(name, false, false, id);
        }

        private void FindAndAnimate(string name, bool toggle,bool open,int id)
        {
            foreach (AnimationGroup g in animations)
            {
                if(g.name == name)
                {
                    if (toggle)
                    {
                        g.Toggle(id);
                        return;
                    }

                    if(open)
                    {
                        g.Open(id);
                        return;
                    }
                    else
                    {
                        g.Close(id);
                        return;
                    }  
                }
            }

            Debug.LogError("Couldn't find a animation group with the name " + name);
        }

        #region holder classes
        //Rect
        [System.Serializable]
        public class RectMoveAnimation
        {
            public RectTransform rect;
            public Vector3 open, close;
        }

        [System.Serializable]
        public class RectScaleAnimation
        {
            public RectTransform rect;
            public Vector3 open, close;
        }

        [System.Serializable]
        public class RectRotateAnimation
        {
            public RectTransform rect;
            public Vector3 open, close;
        }

        [System.Serializable]
        public class TextFadeAnimation
        {
            public TextMeshProUGUI text;
            public Color open = Color.white, close = Color.clear;
        }

        //Image
        [System.Serializable]
        public class ImageFadeAnimation
        {
            public Image image;
            public Color open = Color.white, close = Color.clear;
        }

        [System.Serializable]
        public class ImageFillAnimation
        {
            public Image image;
            public float open = 1, close = 0;
        }

        //Canvas group
        [System.Serializable]
        public class CanvasGroupFadeAnimation
        {
            public CanvasGroup cg;
            public float open = 1, close = 0;
        }



        #endregion

        public struct UIAnimationEvent
        {
            public string name;
            public bool opened;
            public int identifier;

            public UIAnimationEvent(string _name, bool _opened,int _identifier)
            {
                name = _name;
                opened = _opened;
                identifier = _identifier;
            }

            public override string ToString()
            {
                return string.Format("Name: {0}, opened: {1}, identifier: {2}",name,opened, identifier);
            }
        }

        [System.Serializable]
        public class UIAnimEvent : UnityEvent<UIAnimationEvent>
        {
        }
    }
}



