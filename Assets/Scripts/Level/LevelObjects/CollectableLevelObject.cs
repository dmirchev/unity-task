using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    public class CollectableLevelObject : LevelObject
    {
        public override LevelObjectType GetLevelObjectType()
        {
            return LevelObjectType.Collectable;
        }
    }
}