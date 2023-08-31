using Photon.Pun;
using UnityEngine;

namespace Shooter.Combat
{
    public class WeirdBullet : Projectile
    {
        [SerializeField] float amplitude = 1f;             

        Vector3 startPosition;

        private void Start()
        {
            startPosition = transform.position;
        }

        private void Update()
        {
            ProjectileBehaviour();
        }
        internal override void ProjectileBehaviour()
        {
            float verticalPosition = Mathf.Sin(Time.time * speed) * amplitude;
            float horizontalMovement = projectileDirection * speed * Time.deltaTime;
            Vector3 newPosition = transform.position + new Vector3(horizontalMovement, verticalPosition, 0f);

            transform.position = newPosition;
        }
    }
}

