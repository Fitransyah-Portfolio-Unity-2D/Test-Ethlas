
using Photon.Pun;
using UnityEngine;

namespace Shooter.Combat
{
    public class SemiRiffleBullet : Projectile
    {
        [SerializeField] GameObject duplicatePrefab; 
        public override void InitBullet()
        {
            base.InitBullet();

            float offset = 0f;
            for (int i = 0; i < 4; i++)
            {
                offset += 0.75f;

                Vector3 spawnOffset = new Vector3(transform.position.x + offset * projectileDirection, transform.position.y, transform.position.z);
                Gunner gunner = projectileInstigator.GetComponent<Gunner>();
                if (gunner != null)
                {
                    gunner.TriggerFire(spawnOffset, duplicatePrefab, speed);
                }
            }

        }
    }
}




