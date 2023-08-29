using Photon.Pun;
using Shooter.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Shooter.Combat
{
    public class Projectile : MonoBehaviourPun
    {
        [SerializeField] internal float speed = 1f;
        [SerializeField] float MaxLifetime = 22f;
        [SerializeField] float projectileDamage = 0f;
        
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] UnityEvent onHit;

        
        internal float projectileDirection;
        internal GameObject projectileInstigator = null;
        Health target = null;

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
            if (collision.GetComponent<Health>() != null)
            {

                // To prevent self hit
                //if (projectileInstigator.gameObject.name == collision.gameObject.name) return;

                target = collision.GetComponent<Health>();
                target.TakeDamage(projectileInstigator, projectileDamage);

                // Play sound source
                onHit.Invoke();

                if (hitEffect != null)
                {
                    PhotonNetwork.Instantiate(hitEffect.gameObject.name, collision.transform.position, collision.transform.rotation);
                }

                SafeDestroy();

            }
            else
            {
                SafeDestroy();
            }

        }

        private void SafeDestroy()
        {
            PhotonNetwork.Destroy(gameObject);
        }

        public void SetInstigator(GameObject instigator)
        {
            projectileInstigator = instigator;
        }
    }
}


