using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    public class GameUI : Singleton<GameUI>
    {
        [Header("UIs")]
        [SerializeField] private StatsUI statsUI;
        [SerializeField] private LevelUI levelUI;

        [Header("UI Managers")]
        public AspectRatioManager aspectRatioManager;

        public void CreateUIs()
        {
            levelUI.CreateUI();
        }

        void LateUpdate()
        {
            aspectRatioManager.LateUpdateUI();
        }
    }
}