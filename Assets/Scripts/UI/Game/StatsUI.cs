using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    public class StatsUI : MonoBehaviour
    {
        [Header("Content")]
        [SerializeField] private GameObject contentGameObject;

        public void SetContentState(bool state)
        {
            contentGameObject.SetActive(state);
        }

        public void InitUI()
        {

        }
    }
}