using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    public abstract class CollectableLevelObject : StaticLevelObject
    {
        public override LevelObjectType GetLevelObjectType()
        {
            return LevelObjectType.Collectable;
        }

        protected override void Init()
        {
            gameObject.SetActive(true);
        }

        public virtual void ExecuteCollectable()
        {
            gameObject.SetActive(false);
        }

        public void Update()
        {
            transform.GetChild(0).localRotation = Quaternion.AngleAxis(Time.time * Mathf.PI * 20.0f, Vector3.up);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LevelManager.Instance.playerLayer)
                ExecuteCollectable();
        }
    }
}