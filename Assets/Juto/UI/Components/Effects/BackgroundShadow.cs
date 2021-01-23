using System.Collections.Generic;
using UnityEditor;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/Effects/Background  Shadow", 14)]
    public class BackgroundShadow : BaseMeshEffect
    {
        [SerializeField]
        private Color m_EffectColor = new Color(0f, 0f, 0f, 0.5f);

        public Color color2, color3, color4;


        [SerializeField]
        private Vector2 m_EffectDistance = new Vector4(1f, -1f);

        [SerializeField]
        private Vector2 m_EffectSize = new Vector4(1f, -1f);

        [SerializeField]
        private bool m_UseGraphicAlpha = true;

        protected BackgroundShadow()
        { }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            effectDistance = m_EffectDistance;
            effectSize = m_EffectSize;
            base.OnValidate();
        }

#endif

        public Color effectColor
        {
            get { return m_EffectColor; }
            set
            {
                m_EffectColor = value;
                if (graphic != null)
                    graphic.SetVerticesDirty();
            }
        }

        public Vector2 effectDistance
        {
            get { return m_EffectDistance; }
            set
            {

                float x = Mathf.Clamp(value.x, -600, 600);
                float y = Mathf.Clamp(value.y, -600, 600);
                Vector2 n = new Vector2(x, y);

                if (m_EffectDistance == n)
                    return;

                m_EffectDistance = n;

                if (graphic != null)
                    graphic.SetVerticesDirty();
            }
        }

        public Vector2 effectSize
        {
            get { return m_EffectSize; }
            set
            {

                float x = Mathf.Clamp(value.x, -600, 600);
                float y = Mathf.Clamp(value.y, -600, 600);
                Vector2 n = new Vector2(x, y);

                if (m_EffectSize == n)
                    return;

                m_EffectSize = n;

                if (graphic != null)
                    graphic.SetVerticesDirty();
            }
        }

        public bool useGraphicAlpha
        {
            get { return m_UseGraphicAlpha; }
            set
            {
                m_UseGraphicAlpha = value;
                if (graphic != null)
                    graphic.SetVerticesDirty();
            }
        }

        protected void ApplyShadowZeroAlloc(List<UIVertex> verts, Color32 color, int start, int end, float x, float y)
        {
            UIVertex vt;

            var neededCpacity = verts.Count * 2;
            if (verts.Capacity < neededCpacity)
                verts.Capacity = neededCpacity;

            RectTransform rect = GetComponent<RectTransform>();

            float PosFixx = (rect.sizeDelta.x+x);
            float PosFixy = (rect.sizeDelta.y+ y);
            vt = verts[start];
            vt.position += new Vector3(PosFixx, PosFixy, 0);

            for (int i = start; i < end; ++i)
            {
                vt = verts[i];
                verts.Add(vt);

                Vector3 v = vt.position;
                v.x += x + effectSize.x;
                v.y += y + effectSize.y;
                vt.position = v;
                var newColor = color;
                if (m_UseGraphicAlpha)
                    newColor.a = (byte)((newColor.a * verts[i].color.a) / 255);
                vt.color = newColor;
                verts[i] = vt;
            }
        }

        protected void ApplyShadow(List<UIVertex> verts, Color32 color, int start, int end, float x, float y)
        {
            var neededCpacity = verts.Count * 2;
            if (verts.Capacity < neededCpacity)
                verts.Capacity = neededCpacity;

            ApplyShadowZeroAlloc(verts, color, start, end, x, y);
        }

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
                return;

            var output = ListPool<UIVertex>.Get();
            vh.GetUIVertexStream(output);
            
            ApplyShadow(output, effectColor, 0, output.Count, effectDistance.x, effectDistance.y);
            vh.Clear();
            vh.AddUIVertexTriangleStream(output);
            ListPool<UIVertex>.Release(output);
        }
    }
}