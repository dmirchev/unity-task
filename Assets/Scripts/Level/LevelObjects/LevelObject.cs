using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    public abstract class LevelObject : MonoBehaviour
    {
        public abstract LevelObjectType GetLevelObjectType();

        public string uiButtonName;
    }

    public enum LevelObjectType
    {
        Player,
        Obstacle,
        NPC,
        Collectable,

        Count
    }
}