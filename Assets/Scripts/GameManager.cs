using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    public class GameManager : Singleton<GameManager>
    {
        private GameState gameState;

        [HideInInspector] public int coinsCollected;
        private float health;
        public float maxHealth;

        void Start()
        {
            gameState = GameState.None;
            PlayerDataManager.Instance.Create(
                () => {

                    CreateManagers();

                    GameStateEdit();
                }
            );
        }

        public void CreateManagers()
        {
            GridManager.Instance.CreateManager();
            LevelManager.Instance.CreateManager();

            GameUI.Instance.CreateUIs();
        }

        public void ToggleGameState()
        {
            if (gameState == GameState.Play)
                GameStateEdit();
            else
                GameStatePlay();
        }

        public void GameStateEdit()
        {
            gameState = GameState.Edit;

            InitManagers();

            GameUI.Instance.SetGameStateEditUI();
        }

        public void GameStatePlay()
        {
            gameState = GameState.Play;

            InitManagers();

            GameUI.Instance.SetGameStatePlayUI();
        }

        public 

        void InitManagers()
        {
            coinsCollected = 0;

            health = maxHealth;

            LevelManager.Instance.InitManager();
        }

        void Update()
        {
            if (gameState == GameState.Play)
            {
                LevelManager.Instance.UpdateManager();
            }
        }

        void FixedUpdate()
        {
            if (gameState == GameState.Play)
                LevelManager.Instance.FixedUpdateManager();
        }

        public void AddCoin()
        {
            coinsCollected++;

            GameUI.Instance.UpdateCoins(coinsCollected);
        }

        public void ApplyDamage(float damage)
        {
            health -= damage;

            GameUI.Instance.UpdateHealth(health);

            if (health <= 0)
                Finish(false);
        }

        public void Finish(bool success)
        {
            gameState = GameState.None;

            GameUI.Instance.FinishUI(success);
        }
    }

    enum GameState
    {
        None,
        Edit,
        Play
    }
}