using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    public abstract class CollectableLevelObject : StaticLevelObject
    {
        public override LevelObjectType GetLevelObjectType()
        {
            return LevelObjectType.Collectable;
        }

        public virtual void ExecuteCollectable()
        {
            gameObject.SetActive(false);
        }
    }
}