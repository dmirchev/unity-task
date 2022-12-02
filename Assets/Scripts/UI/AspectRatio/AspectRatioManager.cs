using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    [System.Serializable]
    public class AspectRatioManager
    {
        private float lastSceenWidth;
        private float lastSceenHeight;

        private float defaultSceenWidth = 1920.0f;
        private float defaultSceenHeight = 1080.0f;

        private float aspectRatioWidth;
        private float aspectRatioHeight;

        void SetLastScreenSize()
        {
            lastSceenWidth = Screen.width;
            lastSceenHeight = Screen.height;
        }

        public void LateUpdateUI()
        {
            if (Screen.width != lastSceenWidth || Screen.height != lastSceenHeight)
                SetAspectRatio();
        }

        private void SetAspectRatio()
        {
            SetLastScreenSize();

            aspectRatioWidth = Screen.width / defaultSceenWidth;
            aspectRatioHeight = Screen.height / defaultSceenHeight;
        }

        public float GetAspectRatioWidth()
        {
            return aspectRatioWidth;
        }

        public float GetAspectRatioHeight()
        {
            return aspectRatioHeight;
        }
    }
}