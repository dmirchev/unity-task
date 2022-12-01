using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityTask
{
    public class FlexibleGridHolder : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private LayoutElement _layoutElement;

        [SerializeField] protected float height;

        public float Height { get { return height; } }

        void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _layoutElement = GetComponent<LayoutElement>();
        }

        public void SetFlexibleHeight(float sizeY)
        {
            _layoutElement.flexibleHeight = height / sizeY;
        }
    }
}