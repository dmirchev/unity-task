using TMPro;
using UnityEngine;

namespace UnityTask
{
    public class FlexibleGridElement : MonoBehaviour
    {
        [SerializeField] private TMP_Text lableText;

        public void SetGridElement(string lableString)
        {
            lableText.text = lableString;
        }
    }
}