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

        protected override Vector3 GetLocalScale()
        {
            return new Vector3(GridManager.Instance.GridCellSize, 1, GridManager.Instance.GridCellSize);
        }
    }
}