using Photon.Pun;
using UnityEngine;

namespace Shooter.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weaponConfig;

        private void Start()
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer.sprite == null)
            {
                spriteRenderer.sprite = weaponConfig.GetWeaponSprite();
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                Gunner gunner = collision.GetComponent<Gunner>();
                gunner.EquipWeapon(weaponConfig);

                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.Destroy(gameObject);
                }
                
            }
        }
    }
}
