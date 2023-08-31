using Photon.Pun;
using Shooter.Player;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Shooter.Combat
{
    public class Projectile : MonoBehaviourPun
    {
        [SerializeField] internal float speed = 1f;
        [SerializeField] float MaxLifetime = 22f;
        [SerializeField] float projectileDamage = 0f;

        internal float projectileDirection;
        internal GameObject projectileInstigator = null;
        Health target = null;

        public event Action<GameObject> OnCollide;

        private void Start()
        {
            Invoke("SafeDestroy", MaxLifetime);
        }

        private void Update()
        {
            ProjectileBehaviour();
        }

        internal virtual void ProjectileBehaviour()
        {
            transform.Translate(new Vector3(projectileDirection, 0f, 0f) * speed * Time.deltaTime);
        }

        public virtual void InitBullet()
        {
            if (projectileInstigator != null)
            {
                projectileDirection = projectileInstigator.transform.localScale.x;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player" && collision.gameObject != gameObject)
            {
                target = collision.gameObject.GetComponent<Health>();
                if (target != null && target.GetHealthPoints() > 0f)
                {
                    target.TakeDamage(projectileInstigator, projectileDamage);
                }

                if (OnCollide != null)
                {
                    OnCollide(gameObject);
                }
                

            }
            else
            {
                if (OnCollide != null)
                {
                    OnCollide(gameObject);
                }
            }

        }

        public void SetInstigator(GameObject instigator)
        {
            projectileInstigator = instigator;
        }

        public void SetNewSpeed(float duplicateSpeed)
        {
            speed = duplicateSpeed;
        }
    }
}


