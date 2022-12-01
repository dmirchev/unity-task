using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    public class FlexibleGridLayoutHolder : FlexibleGridHolder
    {
        [SerializeField] private FlexibleGridLayout flexibleGridLayout;

        public void AddElementToGrid(Transform element)
        {
            flexibleGridLayout.AddChild(element, out height);
        }

        public float GetHeight()
        {
            return flexibleGridLayout.GetHeight();
        }
    }
}