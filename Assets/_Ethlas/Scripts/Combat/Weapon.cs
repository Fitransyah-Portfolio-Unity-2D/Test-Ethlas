using Photon.Pun;
using UnityEngine;

namespace Shooter.Combat
{
    [CreateAssetMenu(fileName = "NewWeapon", menuName = "Shooter/Weapons/NewWeapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] float weaponCooldownTime;
        [SerializeField] Sprite weaponSprite;
        [SerializeField] Projectile projectile;


        // called by gunner in hit method
        public void LaunchProjectile(GameObject instigator, Gunner shooter)
        {
            GameObject projectileInstance =  PhotonNetwork.Instantiate(projectile.gameObject.name, shooter.GetWeaponSocket().transform.position, shooter.GetWeaponSocket().transform.rotation);

            Transform projectileContainer = GameObject.FindGameObjectWithTag("Container").transform;
            if (projectileContainer != null)
            {
                projectileInstance.transform.SetParent(projectileContainer);
            }

            Projectile projInstanceBehaviour = projectileInstance.GetComponent<Projectile>();
            projInstanceBehaviour.SetInstigator(instigator);
            projInstanceBehaviour.InitBullet();
        }

        public Sprite GetWeaponSprite()
        {
            return weaponSprite;
        }

        public float GetCooldownTime()
        {
            return weaponCooldownTime;
        }
    }
}
