using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    public abstract class UI : MonoBehaviour
    {
        [Header("Content")]
        [SerializeField] private GameObject contentGameObject;

        public abstract void CreateUI();
        public abstract void InitUI();
        public virtual void UpdateUI()  {return; }

        public void SetContentState(bool state)
        {
            contentGameObject.SetActive(state);
        }
    }
}