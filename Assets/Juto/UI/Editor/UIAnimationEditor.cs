using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Juto.UI
{
    [InitializeOnLoad]
    [CustomEditor(typeof(UIAnimation))]
    public class LevelScriptEditor : Editor
    {
        public override void OnInspectorGUI()
        {

            DrawDefaultInspector();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            if (GUILayout.Button("Open all"))
            {
                Toggle(true);
            }

            if (GUILayout.Button("Close all"))
            {
                Toggle(false);
            }
        }

        public void Toggle(bool open)
        {
            UIAnimation myTarget = (UIAnimation)target;

            foreach (AnimationGroup group in myTarget.animations)
            {
                foreach (UIAnimation.RectMoveAnimation item in group.moveAnimations)
                {
                    item.rect.anchoredPosition = (open) ? item.open : item.close;
                }

                foreach (UIAnimation.RectScaleAnimation item in group.scaleAnimations)
                {
                    item.rect.localScale = (open) ? item.open : item.close;
                }

                foreach (UIAnimation.RectRotateAnimation item in group.rotateAnimation)
                {
                    Vector3 rot = (open) ? item.open : item.close;
                    item.rect.rotation = Quaternion.Euler(rot);
                }

                foreach (UIAnimation.TextFadeAnimation item in group.textFadeAnimations)
                {
                    item.text.color = (open) ? item.open : item.close;
                }

                foreach (UIAnimation.ImageFadeAnimation item in group.imageFadeAnimations)
                {
                    item.image.color = (open) ? item.open : item.close;
                }

                foreach (UIAnimation.ImageFillAnimation item in group.imageFillAnimations)
                {
                    item.image.fillAmount = (open) ? item.open : item.close;
                }

                foreach (UIAnimation.CanvasGroupFadeAnimation item in group.canvasGroupFadeAnimation)
                {
                    item.cg.alpha = (open) ? item.open : item.close;
                }

            }
        }
    }
}
