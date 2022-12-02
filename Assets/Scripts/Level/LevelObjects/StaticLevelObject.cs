using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    public abstract class StaticLevelObject : LevelObject
    {
        public override LevelObjectVariant GetLevelObjectVariant()
        {
            return LevelObjectVariant.Static;
        }
    }
}