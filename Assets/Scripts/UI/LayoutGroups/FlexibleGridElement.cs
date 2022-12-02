using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityTask
{
    public class FlexibleGridElement : MonoBehaviour
    {
        [SerializeField] private TMP_Text lableText;
        [SerializeField] private Button button;

        private LevelObjectType levelObjectType;
        public LevelObjectType LevelObjectType { get { return levelObjectType; } }

        public void SetGridElement(string lableString)
        {
            lableText.text = lableString;
        }

        public void SetButtonOnClick(UnityEngine.Events.UnityAction call, LevelObjectType levelObjectType)
        {
            button.onClick.AddListener(call);

            this.levelObjectType = levelObjectType;
        }

        public void SetButtonState(bool interactable)
        {
            button.interactable = interactable;
        }

        public void SetButtonDisabledColor(Color disabledColor)
        {
            ColorBlock colorBlock = button.colors;
            colorBlock.disabledColor = disabledColor;
            button.colors = colorBlock;
        }
    }
}