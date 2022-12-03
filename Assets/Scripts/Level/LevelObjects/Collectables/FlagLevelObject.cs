using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    public class FlagLevelObject : CollectableLevelObject
    {
        public override void ExecuteCollectable()
        {
            base.ExecuteCollectable();
            
            GameManager.Instance.Finish(true);
        }
    }
}