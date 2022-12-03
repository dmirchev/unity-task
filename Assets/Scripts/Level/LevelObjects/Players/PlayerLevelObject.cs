using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    public class PlayerLevelObject : DynamicLevelObject
    {
        public override LevelObjectType GetLevelObjectType()
        {
            return LevelObjectType.Player;
        }

        public override void GetInput()
        {
            Vector2 input = Vector2.zero;

            if (Input.GetKey(KeyCode.W))
            {
                input.x = 1;
                input.y = 1;
            }
            
            if (Input.GetKey(KeyCode.S))
            {
                input.x = -1;
                input.y = -1;
            }

            if (Input.GetKey(KeyCode.A))
            {
                input.x += -1;
                input.y += 1;
            }

            if (Input.GetKey(KeyCode.D))
            {
                input.x += 1;
                input.y += -1;
            }

            input.x = Mathf.Clamp(input.x, -1, 1);
            input.y = Mathf.Clamp(input.y, -1, 1);

            physicsEntity.LeftRightInputDirection = input.x;
            physicsEntity.ForwardBackwardsInputDirection = input.y;

            if (Input.GetKeyDown(KeyCode.Space))
                physicsEntity.JumpInput = true;

            if (Input.GetKeyDown(KeyCode.Q))
                physicsEntity.ResetInput = true;
        }

        public void AddForce(Vector3 position, float hitForce)
        {
            physicsEntity.AddForce(
                (physicsEntity.Position - position).normalized * hitForce
            );
        }
    }
}