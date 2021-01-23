using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Juto.UI
{
    [System.Serializable]
    public class AnimationGroup
    {
        public string name;
        public float time, delay;
        public bool isOpen = false;
        private int id;

        [Space]

        [Header("Animations")]
        public List<UIAnimation.RectMoveAnimation>    moveAnimations;
        public List<UIAnimation.RectScaleAnimation> scaleAnimations;
        public List<UIAnimation.RectRotateAnimation> rotateAnimation;
        public List<UIAnimation.TextFadeAnimation> textFadeAnimations;
        public List<UIAnimation.ImageFadeAnimation> imageFadeAnimations;
        public List<UIAnimation.ImageFillAnimation> imageFillAnimations;
        public List<UIAnimation.CanvasGroupFadeAnimation> canvasGroupFadeAnimation;

        [Header("Events")]
        public UIAnimation.UIAnimEvent OnStart;
        public UIAnimation.UIAnimEvent OnEnd;

        private UIAnimationFramework effect;

        public void Close(int _id)
        {
            id = _id;
            isOpen = false;
            Animate();
        }

        public void Open(int _id)
        {
            id = _id;
            isOpen = true;
            Animate();
        }

        public void Toggle(int _id)
        {
            id = _id;
            isOpen = !isOpen;
            Animate();

        }

        private void Animate()
        {
            if(effect == null)
                effect = GameObject.FindObjectOfType<UIAnimationFramework>();

            OnStart.Invoke(new UIAnimation.UIAnimationEvent(name, isOpen,id));

            if ((moveAnimations.Count + scaleAnimations.Count + rotateAnimation.Count + textFadeAnimations.Count + imageFadeAnimations.Count + imageFillAnimations.Count + canvasGroupFadeAnimation.Count) == 0)
            {
                OnEnd.Invoke(new UIAnimation.UIAnimationEvent(name, isOpen,id));
                return;
            }

            bool first = true;

            foreach (UIAnimation.RectMoveAnimation move in moveAnimations)
            {
                Vector3 point = (isOpen) ? move.open : move.close;

                if(first)
                {
                    //Only fire OnOver event one time.
                    effect.Move(move.rect, point, time,delay, OnOver);
                    first = false;
                }
                else
                    effect.Move(move.rect, point, time);
            }

            foreach (UIAnimation.RectScaleAnimation scale in scaleAnimations)
            {
                Vector3 point = (isOpen) ? scale.open : scale.close;

                if (first)
                {
                    //Only fire OnOver event one time.
                    effect.Scale(scale.rect, point, time, delay, OnOver);
                    first = false;
                }
                else
                    effect.Move(scale.rect, point, time);
            }

            foreach (UIAnimation.RectRotateAnimation rotate in rotateAnimation)
            {
                Vector3 point = (isOpen) ? rotate.open : rotate.close;

                if (first)
                {
                    //Only fire OnOver event one time.
                    effect.Rotate(rotate.rect, Quaternion.Euler(point), time, delay, OnOver);
                    first = false;
                }
                else
                    effect.Rotate(rotate.rect, Quaternion.Euler(point), time);
            }

            foreach (UIAnimation.TextFadeAnimation fade in textFadeAnimations)
            {
                Color color = (isOpen) ? fade.open : fade.close;

                if (first)
                {
                    //Only fire OnOver event one time.
                    effect.Fade(fade.text, color, time, delay, OnOver);
                    first = false;
                }
                else
                    effect.Fade(fade.text, color, time);
            }

            foreach (UIAnimation.ImageFadeAnimation image in imageFadeAnimations)
            {
                Color color = (isOpen) ? image.open : image.close;

                if (first)
                {
                    //Only fire OnOver event one time.
                    effect.Fade(image.image, color, time, delay, OnOver);
                    first = false;
                }
                else
                    effect.Fade(image.image, color, time);
            }

            foreach (UIAnimation.ImageFillAnimation img in imageFillAnimations)
            {
                float value = (isOpen) ? img.open : img.close;

                if (first)
                {
                    //Only fire OnOver event one time.
                    effect.Fill(img.image, value, time, delay, OnOver);
                    first = false;
                }
                else
                    effect.Fill(img.image, value, time);
            }

            foreach (UIAnimation.CanvasGroupFadeAnimation cg in canvasGroupFadeAnimation)
            {
                float visible = (isOpen) ? cg.open : cg.close;

                if (first)
                {
                    //Only fire OnOver event one time.
                    effect.Fade(cg.cg, visible, time, delay, OnOver);
                    first = false;
                }
                else
                    effect.Fade(cg.cg, visible, time);
            }

        }

        public void OnOver()
        {
            OnEnd.Invoke(new UIAnimation.UIAnimationEvent(name,isOpen,id));
        }
    }
}

