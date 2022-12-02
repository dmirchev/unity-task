using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityTask
{
    public class GameUI : Singleton<GameUI>
    {
        [Header("UIs")]
        [SerializeField] private StatsUI statsUI;
        [SerializeField] private LevelUI levelUI;
        [SerializeField] private Button toggleGameStateButton;
        [SerializeField] private TMP_Text gameStateButtonText;

        [Header("UI Managers")]
        public AspectRatioManager aspectRatioManager;

        string GAMESTATEEDITBUTTONTEXT = "Play";
        string GAMESTATEPLAYBUTTONTEXT = "Edit";

        public void CreateUIs()
        {
            levelUI.CreateUI();

            toggleGameStateButton.onClick.AddListener(OnClickGameStateButton);
        }

        public void OnClickGameStateButton()
        {
            GameManager.Instance.ToggleGameState();
        }

        public void SetGameStateEditUI()
        {
            gameStateButtonText.text = GAMESTATEEDITBUTTONTEXT;

            statsUI.SetContentState(false);
            levelUI.SetContentState(true);

            levelUI.InitUI();
        }

        public void SetGameStatePlayUI()
        {
            gameStateButtonText.text = GAMESTATEPLAYBUTTONTEXT;

            statsUI.SetContentState(true);
            levelUI.SetContentState(false);

            levelUI.InitUI();
        }

        public void UpdateLevelUI()
        {
            levelUI.UpdateUI();
        }

        void LateUpdate()
        {
            aspectRatioManager.LateUpdateUI();
        }
    }
}