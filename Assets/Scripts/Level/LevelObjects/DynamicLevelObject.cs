using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    public abstract class DynamicLevelObject : LevelObject
    {
        public override LevelObjectVariant GetLevelObjectVariant()
        {
            return LevelObjectVariant.Dynamic;
        }

        public PhysicsEntity physicsEntity;

        public override void LevelObjectAwake()
        {
            base.LevelObjectAwake();

            physicsEntity.Create();
        }

        public void UpdateLevelObject()
        {
            physicsEntity.Update();
        }

        public void FixedUpdateLevelObject()
        {
            physicsEntity.FixedUpdate();
        }

        public override void InitTransform(Vector3 localPosition)
        {
            base.InitTransform(localPosition);

            transform.localScale = Vector3.one;
        }
    }
}