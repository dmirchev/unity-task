using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    [System.Serializable]
    public class PhysicsEntity
    {
        [SerializeField] private Rigidbody _rigidbody;
        public Vector3 Position { get { return _rigidbody.position; } }
        [SerializeField] private CapsuleCollider _capsuleCollider;
        [SerializeField] private Transform _modelTransform;

        private LayerMask groundLayerMask;
        private LayerMask obstacleLayerMask;

        public LayerMask ObstacleLayerMask { get { return obstacleLayerMask; } }

        [SerializeField] private Vector3 velocity;
        public Vector3 Velocity { get { return velocity; } }
        [SerializeField] private float maxVelocity;
        public float MaxVelocity { get { return maxVelocity; } }

        [SerializeField] private float maxSpeed;
        public float MaxSpeed { get { return maxSpeed; } }

        [SerializeField] private float acceleration;
        [SerializeField] private float deceleration;

        [SerializeField] private float jumpForce;
        [SerializeField] private float gravity;
        [SerializeField] private float gravityClamp;

        public float rotaionSmoothSpeed = 1.0f;

        [SerializeField] float forwardBackwardsInputDirection;
        [SerializeField] float leftRightInputDirection;

        public float ForwardBackwardsInputDirection { set { forwardBackwardsInputDirection = value; } }
        public float LeftRightInputDirection { set { leftRightInputDirection = value; } }

        bool jumpInput;
        public bool JumpInput { set { jumpInput = value; } }

        bool resetInput;
        public bool ResetInput { set { resetInput = value; } }

        [Header("Direction Driven")]
        public Vector3 directionVelocity;

        public void Create()
        {
            InitCastArrays();

            groundLayerMask = (1 << LevelManager.GROUNDLAYER) | (1 << LevelManager.Instance.obstacleLayer);
            obstacleLayerMask = 1 << LevelManager.Instance.obstacleLayer;
        }

        void InitCastArrays()
        {
            groundHits = new RaycastHit[1];

            wallCollider = new Collider[4];
            wallColliderIntersections = new Vector3[4];
        }

        public void Init()
        {
            InitCastArrays();

            velocity = Vector3.zero;
            velocityFloat = 0;

            directionVelocity = Vector3.back;

            RotateModel(directionVelocity);
        }

        public void Update()
        {
            DirectionUpdate();
        }

        public void FixedUpdate()
        {
            Movement();
        }

        void RotateModel(Vector3 direction)
        {
            float angle = Mathf.Atan2(direction.normalized.x, direction.normalized.z);
            Quaternion nextRotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.up);
            _modelTransform.localRotation = Quaternion.LerpUnclamped(_modelTransform.localRotation, nextRotation, Time.fixedDeltaTime * rotaionSmoothSpeed);
        }

        [SerializeField] private float directionVelocityMinSpeed = 1;
        [SerializeField] private float directionVelocityMaxSpeed = 1;

        private float velocityFloat;

        void DirectionUpdate()
        {
            if (leftRightInputDirection != 0 || forwardBackwardsInputDirection != 0)
            {
                Vector2 oldDirection = new Vector2(directionVelocity.x, directionVelocity.z);
                Vector2 newDirection = new Vector2(leftRightInputDirection, forwardBackwardsInputDirection);

                float oldAngle = Mathf.Atan2(oldDirection.x, oldDirection.y);
                float newAngle = Mathf.Atan2(newDirection.x, newDirection.y);

                float angleDifferenceRatio = (newAngle-oldAngle) / Mathf.PI;

                // Values from -1 to 1
                float dot = Vector2.Dot(oldDirection, newDirection);
                
                // Values from -1 to 0
                float dotMin = Mathf.Min(dot, 0);

                float directionSpeed = Mathf.Lerp(directionVelocityMinSpeed, directionVelocityMaxSpeed, -dotMin);

                directionVelocity.x = Mathf.Lerp(directionVelocity.x, leftRightInputDirection, directionSpeed * Time.deltaTime);
                directionVelocity.z = Mathf.Lerp(directionVelocity.z, forwardBackwardsInputDirection, directionSpeed * Time.deltaTime);
            }

            directionVelocity.x = Mathf.Clamp(directionVelocity.x, -MaxVelocity, MaxVelocity);
            directionVelocity.z = Mathf.Clamp(directionVelocity.z, -MaxVelocity, MaxVelocity);
        }

        void Movement()
        {
            // Acceleration
            if (leftRightInputDirection != 0 || forwardBackwardsInputDirection != 0)
            {
                velocityFloat += acceleration * Time.fixedDeltaTime;
            }
            else
            {
                velocityFloat += deceleration * Time.fixedDeltaTime;
            }

            velocityFloat = Mathf.Clamp(velocityFloat, 0, maxSpeed);

            velocity.x = directionVelocity.x * velocityFloat;
            velocity.z = directionVelocity.z * velocityFloat;

            // Reset Direction
            leftRightInputDirection = 0;
            forwardBackwardsInputDirection = 0;

            if (hit)
            {
                hit = false;

                velocity += hitVector;
            }

            if (jumpInput)
            {
                jumpInput = false;

                Jump();
            }

            Vector3 nextPosition = _rigidbody.position + velocity * Time.fixedDeltaTime;

            isGrounded = CheckGround(nextPosition);

            if (!isGrounded)
            {
                velocity.y += gravity * Time.fixedDeltaTime;
            }
            else
            {
                // Fix to Ground When We are Falling
                if (velocity.y < 0)
                    velocity.y = (GetColliderTopYPoint(groundHits[0].collider) - _rigidbody.position.y) / Time.fixedDeltaTime;
            }

            velocity.y = Mathf.Clamp(velocity.y, gravityClamp, jumpForce);

            nextPosition = _rigidbody.position + velocity * Time.fixedDeltaTime;

            velocity += CheckWall(nextPosition);

            nextPosition = _rigidbody.position + velocity * Time.fixedDeltaTime;

            // Movement
            _rigidbody.MovePosition(nextPosition);

            RotateModel(directionVelocity);

            if (resetInput)
            {
                resetInput = false;
                velocity = Vector3.zero;
                _rigidbody.MovePosition(Vector3.up * 5);
            }
        }

        bool isGrounded;

        void Jump()
        {
            velocity.y += jumpForce;
        }

        bool hit;
        Vector3 hitVector;

        public void AddForce(Vector3 direction)
        {
            hit = true;
            hitVector = direction * maxSpeed;
        }

        [SerializeField] private float groundRayYOffset;

        RaycastHit[] groundHits;

        bool CheckGround(Vector3 castPosition)
        {
            float halfPlayerHeight = _capsuleCollider.bounds.size.y * 0.5f;
            
            return Physics.RaycastNonAlloc(castPosition + Vector3.up * halfPlayerHeight, Vector3.down, groundHits, halfPlayerHeight + groundRayYOffset, groundLayerMask) > 0;
        }

        float GetColliderTopYPoint(Collider collider)
        {
            return collider.bounds.center.y + collider.bounds.size.y * 0.5f;
        }

        Collider[] wallCollider;
        Vector3[] wallColliderIntersections;

        Vector3 CheckWall(Vector3 castPosition)
        {
            Vector3 point0, point1;
            GetCapsileCenters(castPosition, out point0, out point1);

            int count = Physics.OverlapCapsuleNonAlloc(point0, point1, _capsuleCollider.radius, wallCollider, obstacleLayerMask);

            // Get Intersections
            for (int i = 0; i < count; i++)
            {
                GetCollisionEdge(castPosition, wallCollider[i], i);
                
            }

            Vector3 oppositeWallVector = Vector3.zero;

            // Sort Intersections
            Vector2 dotVector2I, dotVector2J;
            bool hasBreakedInner = false;
            for (int i = 0; i < count; i++)
            {
                hasBreakedInner = false;
                for (int j = 0; j < count; j++)
                {
                    if (j == i)
                        continue;

                    dotVector2I.x = wallColliderIntersections[i].x;
                    dotVector2I.y = wallColliderIntersections[i].z;

                    dotVector2J.x = wallColliderIntersections[j].x;
                    dotVector2J.y = wallColliderIntersections[j].z;

                    dotVector2I = dotVector2I.normalized;
                    dotVector2J = dotVector2J.normalized;

                    // Find Aligning Forces
                    float dot = Vector2.Dot(dotVector2I, dotVector2J);

                    if (dot > 0)
                    {
                        float dot21 = Vector2.Dot(dotVector2I, Vector2.right);
                        float dot22 = Vector2.Dot(dotVector2I, Vector2.up);

                        // Discard Forces Not Parallel
                        if (!(dot21 == 0 || dot22 == 0))
                        {
                            hasBreakedInner = true;
                            break;
                        }
                    }
                }

                if (!hasBreakedInner)
                    oppositeWallVector += wallColliderIntersections[i];
            }

            return -oppositeWallVector / Time.fixedDeltaTime;
        }

        public void GetCapsileCenters(Vector3 castPosition, out Vector3 point0, out Vector3 point1)
        {
            point0 = castPosition + Vector3.up * _capsuleCollider.radius;
            point1 = castPosition + Vector3.up * (_capsuleCollider.bounds.size.y - _capsuleCollider.radius);
        }

        void GetCollisionEdge(Vector3 castPosition, Collider collider, int index)
        {
            castPosition.y += _capsuleCollider.radius;

            BoxCollider boxCollider = (BoxCollider) collider;

            // Find Nearest Point on Edge on Rectangle
            Vector3 boxTopRightEdge = boxCollider.bounds.center - boxCollider.bounds.size * 0.5f;
            Vector3 boxBottomLeftEdge = boxCollider.bounds.center + boxCollider.bounds.size * 0.5f;

            float NearestX = Mathf.Max(boxTopRightEdge.x, Mathf.Min(castPosition.x, boxBottomLeftEdge.x));
            float NearestY = Mathf.Max(boxTopRightEdge.y, Mathf.Min(castPosition.y, boxBottomLeftEdge.y));
            float NearestZ = Mathf.Max(boxTopRightEdge.z, Mathf.Min(castPosition.z, boxBottomLeftEdge.z));

            Vector3 nearestPoint = new Vector3(NearestX, NearestY, NearestZ);

            // Find Nearest Point on Edge on Circle
            Vector3 directionFromCastPositionToNearestPoint = (nearestPoint - castPosition).normalized;
            Vector3 nearestOnCircle = castPosition + directionFromCastPositionToNearestPoint * _capsuleCollider.radius;

            // Intesection
            Vector3 intersectiorn = nearestOnCircle - nearestPoint;

            wallColliderIntersections[index] = intersectiorn;
        }
    }
}