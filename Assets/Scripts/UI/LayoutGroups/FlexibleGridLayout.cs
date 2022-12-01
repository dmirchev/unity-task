using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityTask
{
    public class FlexibleGridLayout : GridLayoutGroup
    {
        public Vector2 DEFAULTCELLSIZE = new Vector2(260, 260);
        public Vector2 DEFAULTSPACING = new Vector2(24, 24);
        public int length = 6;

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            // Error In Editor because of Singleton
            if (GameUI.Instance == null) return;

            float aspectRatioHeight = GameUI.Instance.aspectRatioManager.GetAspectRatioHeight();

            cellSize = DEFAULTCELLSIZE * aspectRatioHeight;
            spacing = DEFAULTSPACING * aspectRatioHeight;

            float contentRectTransformX = rectTransform.rect.size.x;
            if (contentRectTransformX <= 0) return;
            
            float currentXLength = length * cellSize.x + spacing.x;
            float newRatio = 1.0f;
            
            if (contentRectTransformX < currentXLength)
                newRatio = contentRectTransformX / currentXLength;

            cellSize *= newRatio;
            spacing *= newRatio;
        }

        public float GetHeight()
        {
            return cellSize.y + spacing.y;
        } 

        public void AddChild(Transform child, out float height)
        {
            child.SetParent(rectTransform);

            height = Mathf.CeilToInt((float)rectTransform.childCount / length) * GetHeight();
        }
    }
}