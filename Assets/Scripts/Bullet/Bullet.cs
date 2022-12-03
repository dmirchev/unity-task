using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    public class Bullet : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        [SerializeField] private float speed;

        [SerializeField] private float aliveTime;
        private float aliveTimer;

        [Header("Damage")]
        [SerializeField] private float playerHitForce;
        [SerializeField] private float playerDamage;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        void Update()
        {
            aliveTimer += Time.deltaTime;

            if (aliveTimer > aliveTime)
                Destroy(this.gameObject);
        }

        void FixedUpdate()
        {
            _rigidbody.MovePosition(_rigidbody.position + transform.forward * speed * Time.fixedDeltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LevelManager.Instance.playerLayer)
            {
                GameManager.Instance.ApplyDamage(playerDamage);
                other.GetComponent<PlayerLevelObject>().AddForce(_rigidbody.position, playerHitForce);
                Destroy(this.gameObject);
            }
        }
    }
}