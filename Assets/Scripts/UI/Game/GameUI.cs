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
        [SerializeField] private Button gameStateButton;
        [SerializeField] private TMP_Text gameStateButtonText;

        [Header("UI Managers")]
        public AspectRatioManager aspectRatioManager;

        string GAMESTATEEDITBUTTONTEXT = "Play";
        string GAMESTATEPLAYBUTTONTEXT = "Edit";

        [Header("Finish")]
        [SerializeField] private GameObject finishUI;
        [SerializeField] private TMP_Text finishUIText;
        [SerializeField] private Button finishUIButton;

        public void CreateUIs()
        {
            levelUI.CreateUI();
            statsUI.CreateUI();

            gameStateButton.onClick.AddListener(OnClickGameStateButton);
            finishUIButton.onClick.AddListener(GameManager.Instance.GameStateEdit);
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

            finishUI.SetActive(false);

            levelUI.InitUI();
        }

        public void SetGameStatePlayUI()
        {
            gameStateButtonText.text = GAMESTATEPLAYBUTTONTEXT;

            statsUI.SetContentState(true);
            levelUI.SetContentState(false);

            finishUI.SetActive(false);

            statsUI.InitUI();
        }

        public void CheckGameStateButtonState()
        {
            gameStateButton.interactable = LevelManager.Instance.hasPlayer;
        }

        public void UpdateLevelUI()
        {
            levelUI.UpdateUI();

            CheckGameStateButtonState();
        }

        void LateUpdate()
        {
            aspectRatioManager.LateUpdateUI();
        }

        public void UpdateCoins(int coins)
        {
            statsUI.UpdateCoins(coins);
        }

        public void UpdateHealth(float health)
        {
            statsUI.UpdateHealth(health);
        }

        public void FinishUI(bool success)
        {
            finishUI.SetActive(true);

            finishUIText.text = success ? "Success" : "Fail";
        }
    }
}