using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    public abstract class LevelObject : MonoBehaviour
    {
        public abstract LevelObjectType GetLevelObjectType();
        public abstract LevelObjectVariant GetLevelObjectVariant();

        public string label;

        void Awake() { LevelObjectAwake(); }

        public virtual void LevelObjectAwake() { return; }

        public virtual void InitTransform(Vector3 localPosition)
        {
            transform.localPosition = localPosition;
            transform.localRotation = Quaternion.identity;
            transform.localScale = GridManager.Instance.GridCellScale;
        }
    }

    public enum LevelObjectType
    {
        None,
        Player,
        Obstacle,
        NPC,
        Collectable,

        Count
    }

    public enum LevelObjectVariant
    {
        Static,
        Dynamic,

        Count
    }
}