using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    public class ObstacleLevelObject : StaticLevelObject
    {
        public override LevelObjectType GetLevelObjectType()
        {
            return LevelObjectType.Obstacle;
        }
    }
}