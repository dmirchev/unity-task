using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    public class PlayerLevelObject : DynamicLevelObject
    {
        public override LevelObjectType GetLevelObjectType()
        {
            return LevelObjectType.Player;
        }
    }
}