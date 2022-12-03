using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityTask
{
    public class StatsUI : UI
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private TMP_Text coinsText;

        public override void CreateUI()
        {
            healthSlider.maxValue = GameManager.Instance.maxHealth;
        }

        public override void InitUI()
        {
            UpdateHealth(GameManager.Instance.maxHealth);
            UpdateCoins(0);
        }

        public void UpdateHealth(float health)
        {
            healthSlider.value = health;
        }

        public void UpdateCoins(int coins)
        {
            coinsText.text = coins.ToString();
        }
    }
}