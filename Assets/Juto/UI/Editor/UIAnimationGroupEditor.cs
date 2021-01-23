using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Reflection;
using UnityEditorInternal;

namespace Juto.UI
{
    [CustomPropertyDrawer(typeof(AnimationGroup))]
    public class AnimationGroupEditor : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            EditorGUI.PropertyField(position, property, label, true);

            
            if (property.isExpanded)
            {
                if (GUILayout.Button("Open"))
                {
                    Toggle(true, property);
                }

                if (GUILayout.Button("Close"))
                {
                    Toggle(false, property);

                }

                if (GUILayout.Button("Copy values to open"))
                {
                    CopyValues(true, property);
                }

                if (GUILayout.Button("Copy values to close"))
                {
                    CopyValues(false, property);
                }
            }
            

        }


        /// <summary>
        /// Everything belows are helpers
        /// </summary>
        public void CopyValues(bool toOpen, SerializedProperty property)
        {
            AnimationGroup target = PropertyDrawerUtility.GetActualObjectForSerializedProperty<AnimationGroup>(fieldInfo, property);

            foreach (UIAnimation.RectMoveAnimation item in target.moveAnimations)
            {
                if (toOpen)
                    item.open = item.rect.anchoredPosition;
                else
                    item.close = item.rect.anchoredPosition;
            }

            foreach (UIAnimation.RectRotateAnimation item in target.rotateAnimation)
            {
                Vector3 rot = item.rect.rotation.eulerAngles;

                if (toOpen)
                    item.open = rot;
                else
                    item.close = rot;
            }

            foreach (UIAnimation.RectScaleAnimation item in target.scaleAnimations)
            {
                if (toOpen)
                    item.open = item.rect.localScale;
                else
                    item.close = item.rect.localScale;
            }

            foreach (UIAnimation.TextFadeAnimation item in target.textFadeAnimations)
            {
                if (toOpen)
                    item.open = item.text.color;
                else
                    item.close = item.text.color;
            }

            foreach (UIAnimation.ImageFadeAnimation item in target.imageFadeAnimations)
            {
                if (toOpen)
                    item.open = item.image.color;
                else
                    item.close = item.image.color;
            }

            foreach (UIAnimation.ImageFillAnimation item in target.imageFillAnimations)
            {
                if (toOpen)
                    item.open = item.image.fillAmount;
                else
                    item.close = item.image.fillAmount;
            }

            foreach (UIAnimation.CanvasGroupFadeAnimation item in target.canvasGroupFadeAnimation)
            {
                if (toOpen)
                    item.open = item.cg.alpha;
                else
                    item.close = item.cg.alpha;
            }

        }

        public void Toggle(bool open, SerializedProperty property)
        {
            AnimationGroup target = PropertyDrawerUtility.GetActualObjectForSerializedProperty<AnimationGroup>(fieldInfo, property);

            target.isOpen = open;

            foreach (UIAnimation.RectMoveAnimation item in target.moveAnimations)
            {
                item.rect.anchoredPosition = (open) ? item.open : item.close;
            }

            foreach (UIAnimation.RectScaleAnimation item in target.scaleAnimations)
            {
                item.rect.localScale = (open) ? item.open : item.close;
            }

            foreach (UIAnimation.RectRotateAnimation item in target.rotateAnimation)
            {
                Vector3 rot = (open) ? item.open : item.close;
                item.rect.rotation = Quaternion.Euler(rot);
            }

            foreach (UIAnimation.TextFadeAnimation item in target.textFadeAnimations)
            {
                item.text.color = (open) ? item.open : item.close;
            }

            foreach (UIAnimation.ImageFadeAnimation item in target.imageFadeAnimations)
            {
                item.image.color = (open) ? item.open : item.close;
            }

            foreach (UIAnimation.ImageFillAnimation item in target.imageFillAnimations)
            {
                item.image.fillAmount = (open) ? item.open : item.close;
            }

            foreach (UIAnimation.CanvasGroupFadeAnimation item in target.canvasGroupFadeAnimation)
            {
                item.cg.alpha = (open) ? item.open : item.close;
                item.cg.interactable = open;
                item.cg.blocksRaycasts = open;
            }
        }

    }
}
