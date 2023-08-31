using Photon.Pun;
using UnityEngine;

namespace Shooter.Combat
{
    [CreateAssetMenu(fileName = "NewWeapon", menuName = "Shooter/Weapons/NewWeapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] float weaponCooldownTime;
        [SerializeField] Sprite weaponSprite;
        [SerializeField] GameObject projectile;

        public GameObject GetProjectilePrefab()
        {
            return projectile;
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
