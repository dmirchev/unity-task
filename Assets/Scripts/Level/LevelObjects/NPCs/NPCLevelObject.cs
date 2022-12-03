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

        [Header("Target")]
        public Transform target;

        [Header("Steering Amounts")]
        [SerializeField] private float seekAmount;

        protected override void Init()
        {
            base.Init();

            target = LevelManager.Instance.Player;
        }

        public override void GetInput()
        {
            Vector3 steeringForce = Calculate();

            Debug.DrawRay(physicsEntity.Position, steeringForce, Color.red);

            physicsEntity.ForwardBackwardsInputDirection = Mathf.RoundToInt(steeringForce.z);
            physicsEntity.LeftRightInputDirection = Mathf.RoundToInt(steeringForce.x);
        }

        private Vector3 Calculate()
        {
            Vector3 steeringForce = Vector3.zero;
            Vector3 targetPosition = target.localPosition;
            targetPosition.y = 0;

            if (target != null)
                steeringForce += Seek(targetPosition) * seekAmount;

            return Truncate(steeringForce, physicsEntity.MaxVelocity);
        }

        Vector3 Seek(Vector3 targetPosition)
        {
            Vector3 desiredVelocity = (targetPosition - physicsEntity.Position).normalized * physicsEntity.MaxSpeed;
            return desiredVelocity - physicsEntity.Velocity;
        }

        Vector3 Truncate(Vector3 vector, float max)
        {
            float vectorMagnitude = vector.magnitude;
            Vector3 vectorNormalized = vector.normalized;

            return vectorNormalized * Mathf.Min(vectorMagnitude, max);
        }
    }
}