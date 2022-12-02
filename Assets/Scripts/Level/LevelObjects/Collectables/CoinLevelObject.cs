using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    public class CoinLevelObject : CollectableLevelObject
    {
        public override void ExecuteCollectable()
        {
            base.ExecuteCollectable();

            Debug.Log("HERE");
        }
    }
}