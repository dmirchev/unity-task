using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    public class MineLevelObject : ObstacleLevelObject
    {
        public bool wasTriggered;

        protected override void Init()
        {
            wasTriggered = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LevelManager.Instance.playerLayer)
                GameManager.Instance.Finish(false);
        }
    }
}