using Photon.Pun;
using System;
using UnityEngine;

namespace Shooter.Combat
{
    public class Gunner : MonoBehaviourPun
    {
        [SerializeField] float timeBetweenAttacks = 1f;

        float timeSinceLastAttack = Mathf.Infinity;
        [SerializeField] GameObject weaponSocket;
        [SerializeField] Weapon defaultWeapon;
        [SerializeField] Weapon equippedWeapon;

        Sprite equippedWeaponSprite;

        public event Action OnAttackTriggered;

        private void Start()
        {
            EquipWeapon(defaultWeapon);
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            
            if (Input.GetKeyDown(KeyCode.O))
            {
                if (photonView.IsMine)
                {
                    AttackBehaviour();
                }
            }
        }

        public void EquipWeapon(Weapon newWeapon)
        {
            equippedWeapon = newWeapon;
            equippedWeaponSprite = equippedWeapon.GetWeaponSprite();
            if (equippedWeapon != null && weaponSocket != null)
            {
                timeBetweenAttacks = equippedWeapon.GetCooldownTime();
                GetComponent<PhotonView>().RPC("AttachWeapon", RpcTarget.All);
            }
            else
            {
                print($"Equip weapon is null? {equippedWeapon == null}");
                print($"Socket weapon is null? {weaponSocket == null}");
            }

        }
        [PunRPC]
        private void AttachWeapon()
        {
            SpriteRenderer spriteRenderer = weaponSocket.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = equippedWeaponSprite;
        }

        // input system here

        private void AttackBehaviour()
        {
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        void TriggerAttack()
        {
            equippedWeapon.LaunchProjectile(gameObject, this);
            OnAttackTriggered();
        }

        public GameObject GetWeaponSocket()
        {
            return weaponSocket;
        }

        public Weapon GetDefaultWeapon()
        {
            return defaultWeapon;
        }
    }
}
