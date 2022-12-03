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
        [SerializeField] private float wanderAmount;
        [SerializeField] private float wallAvoidanceAmount;

        [Header("Wander")]
        public float wanderRadius;
        public float wanderDistance;
        public float wanderJitter;
        public Vector3 wanderTarget;

        float GetRandomWander()
        {
            return Random.Range(-1.0f, 1.0f);
        }

        [Header("Damage")]
        [SerializeField] private float playerHitForce;
        [SerializeField] private float playerDamage;
        

        protected override void Init()
        {
            base.Init();

            target = LevelManager.Instance.Player;

            wallRaycastHits = new RaycastHit[1];
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
                
            steeringForce += Wander() * wanderAmount;
            steeringForce += WallAvoidance() * wallAvoidanceAmount;

            return Truncate(steeringForce, physicsEntity.MaxVelocity);
        }

        Vector3 Seek(Vector3 targetPosition)
        {
            Vector3 desiredVelocity = (targetPosition - physicsEntity.Position).normalized * physicsEntity.MaxSpeed;
            return desiredVelocity - physicsEntity.Velocity;
        }

        float randomTimer;
        public float radnomTime;

        Vector3 Wander()
        {
            randomTimer += Time.deltaTime;

            if (randomTimer > radnomTime)
            {
                randomTimer = 0;
                wanderTarget = new Vector3(GetRandomWander() * wanderJitter, 0.0f, GetRandomWander() * wanderJitter);
            }

            wanderTarget = wanderTarget.normalized;
            wanderTarget *= wanderRadius;

            Vector3 targetLocal = wanderTarget * wanderDistance;
            Vector3 targetWorld = PointToWorldSpace(targetLocal, physicsEntity.Position, transform.forward);

            return targetWorld - physicsEntity.Position;
        }

        Vector3 PointToWorldSpace(Vector3 target, Vector3 refPosition, Vector3 refDirection)
        {
            Quaternion rotation = Quaternion.LookRotation(refDirection);
            return refPosition + rotation * target;
        }

        public float rayCastWallDistance;
        RaycastHit[] wallRaycastHits = new RaycastHit[1];

        Vector3 WallAvoidance()
        {
            Vector3 point0, point1;
            physicsEntity.GetCapsileCenters(physicsEntity.Position, out point0, out point1);

            int count = Physics.RaycastNonAlloc(point0, physicsEntity.Velocity.normalized, wallRaycastHits, rayCastWallDistance, physicsEntity.ObstacleLayerMask);

            if (count > 0)
                return Seek(wallRaycastHits[0].point + wallRaycastHits[0].normal * rayCastWallDistance);

            return Vector3.zero;
        }

        Vector3 Truncate(Vector3 vector, float max)
        {
            float vectorMagnitude = vector.magnitude;
            Vector3 vectorNormalized = vector.normalized;

            return vectorNormalized * Mathf.Min(vectorMagnitude, max);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LevelManager.Instance.playerLayer)
            {
                GameManager.Instance.ApplyDamage(playerDamage);
                other.GetComponent<PlayerLevelObject>().AddForce(physicsEntity.Position, playerHitForce);
            }
        }
    }
}