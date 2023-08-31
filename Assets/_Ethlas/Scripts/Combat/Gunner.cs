using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

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

        public InputAction fireAction;
        public event Action OnAttackTriggered;

        private void Awake()
        {
            fireAction.started += OnFire;
        }

        private void OnEnable()
        {
            fireAction.Enable();
        }

        private void OnDisable()
        {
            fireAction.Enable();
        }

        private void Start()
        {
            EquipWeapon(defaultWeapon);
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
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

        private void OnFire(InputAction.CallbackContext context)
        {
            if (!photonView.IsMine) return;
            
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                TriggerFire();
                timeSinceLastAttack = 0;
            }
        }

        void TriggerFire()
        {
            int viewID = GetComponent<PhotonView>().ViewID;
            equippedWeapon.LaunchProjectile(gameObject, this, viewID);
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
