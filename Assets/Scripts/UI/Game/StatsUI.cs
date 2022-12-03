using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityTask
{
    public class StatsUI : MonoBehaviour
    {
        [Header("Content")]
        [SerializeField] private GameObject contentGameObject;
        
        [SerializeField] private Slider healthSlider;
        [SerializeField] private TMP_Text coinsText;

        public void SetContentState(bool state)
        {
            contentGameObject.SetActive(state);
        }

        public void CreateUI()
        {
            healthSlider.maxValue = GameManager.Instance.maxHealth;
        }

        public void InitUI()
        {
            UpdateHealth(1);
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