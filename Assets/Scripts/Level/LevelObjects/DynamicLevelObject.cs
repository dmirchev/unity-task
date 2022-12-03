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

        protected override void Init()
        {
            physicsEntity.Init();
        }

        public abstract void GetInput();

        public void UpdateLevelObject()
        {
            GetInput();
            physicsEntity.Update();
        }

        public void FixedUpdateLevelObject()
        {
            physicsEntity.FixedUpdate();
        }
    }
}