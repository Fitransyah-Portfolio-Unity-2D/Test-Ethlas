
using Photon.Pun;
using UnityEngine;

namespace Shooter.Combat
{
    public class SemiRiffleBullet : Projectile
    {
        [SerializeField] GameObject duplicatePrefabs;

        private void Start()
        {
            //float offset = 0f;
            //for (int i = 0; i < 4; i++)
            //{
            //    offset += 0.75f;

            //    Vector3 spawnOffset = new Vector3(transform.position.x + offset * projectileDirection, transform.position.y, transform.position.z);
            //    Projectile instance = PhotonNetwork.Instantiate(duplicatePrefabs.name, spawnOffset, Quaternion.identity).GetComponent<Projectile>();

            //    Transform projectileContainer = GameObject.FindGameObjectWithTag("Container").transform;
            //    if (projectileContainer != null)
            //    {
            //        instance.transform.SetParent(projectileContainer);
            //    }

            //    instance.speed = speed;
            //    instance.InitBullet();
            //}
        }

        public override void InitBullet()
        {
            base.InitBullet();

            float offset = 0f;
            for (int i = 0; i < 4; i++)
            {
                offset += 0.75f;

                Vector3 spawnOffset = new Vector3(transform.position.x + offset * projectileDirection, transform.position.y, transform.position.z);
                Projectile instance = PhotonNetwork.Instantiate(duplicatePrefabs.name, spawnOffset, Quaternion.identity).GetComponent<Projectile>();

                Transform projectileContainer = GameObject.FindGameObjectWithTag("Container").transform;
                if (projectileContainer != null)
                {
                    instance.transform.SetParent(projectileContainer);
                }

                instance.projectileInstigator = projectileInstigator;
                instance.speed = speed;
                instance.InitBullet();
            }

        }
    }
}




