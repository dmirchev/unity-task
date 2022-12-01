using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    public class ObstacleLevelObject : LevelObject
    {
        public override LevelObjectType GetLevelObjectType()
        {
            return LevelObjectType.Obstacle;
        }
    }
}