using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    public class NPCLevelObject : DynamicLevelObject
    {
        public override LevelObjectType GetLevelObjectType()
        {
            return LevelObjectType.NPC;
        }
    }
}