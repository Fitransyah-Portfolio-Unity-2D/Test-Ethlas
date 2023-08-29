using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shooter.Player
{
    public class Mover : MonoBehaviourPun, IPunObservable
    {
        [SerializeField] float moveSpeed = 5f;
        [SerializeField] float crouchSpeed = 2.5f; // Half the speed when crouching
        [SerializeField] float jumpForce = 10f;
        [SerializeField] float lagInterpolationSpeed = 10f;

        public int jumpsRemaining = 2;

        public InputAction moveAction;
        public InputAction jumpAction;

        Rigidbody2D playerRigidbody;
        CapsuleCollider2D playerCollider;

        public event Action OnMoving;
        public event Action OnStopMoving;
        public event Action OnJumpUp;
        public event Action OnJumpDown;
        public event Action OnLanding;

        private void Awake()
        {
            playerRigidbody = GetComponent<Rigidbody2D>();
            playerCollider = GetComponent<CapsuleCollider2D>(); 

            moveAction.performed += OnMove;
            moveAction.canceled += OnMoveCanceled;
            jumpAction.started += OnJump;

        }

        private void OnEnable()
        {
            moveAction.Enable();
            jumpAction.Enable();
        }

        private void OnDisable()
        {
            moveAction.Disable();
            jumpAction.Disable();
        }

        private void Update()
        {
            if (photonView.IsMine)
            {
                Move();  
            }
        }

        private void FixedUpdate()
        {
            if (playerRigidbody.velocity.y < -0.05f)
            {
                OnJumpDown();
            }
        }

        private void OnMove(InputAction.CallbackContext context)
        {

            if (context.performed && photonView.IsMine)
            {
                Vector2 moveInput = context.ReadValue<Vector2>();
                Vector2 movement = new Vector2(moveInput.x * moveSpeed , playerRigidbody.velocity.y);
                playerRigidbody.velocity = movement;

                if (moveInput.x != 0) // Check for any horizontal input
                {
                    playerRigidbody.transform.localScale = new Vector3(Mathf.Sign(moveInput.x), 1f, 1f); // Flip based on input direction

                    if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
                    {
                        OnMoving();
                    }
                }
            }
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            if (photonView.IsMine)
            {
                playerRigidbody.velocity = new Vector2(0f, playerRigidbody.velocity.y);
                OnStopMoving();
            }
        }


        private void Move()
        {
            float horizontalInput = moveAction.ReadValue<Vector2>().x;
            Vector2 movement = new Vector2(horizontalInput * moveSpeed , playerRigidbody.velocity.y);
            playerRigidbody.velocity = movement;

            if (horizontalInput != 0)
            {
                playerRigidbody.transform.localScale = new Vector3(Mathf.Sign(horizontalInput), playerRigidbody.transform.localScale.y, playerRigidbody.transform.localScale.z);
            }
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            if (photonView.IsMine)
            {
                OnJumpUp();
                if (jumpsRemaining > 0)
                {
                    playerRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    jumpsRemaining--;
                }
                
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Ground")
            {
                jumpsRemaining = 2;
                
                OnLanding();
            }
        }


        #region LagCompensation
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(playerRigidbody.position);
                stream.SendNext(playerRigidbody.rotation);
                stream.SendNext(playerRigidbody.velocity);
            }
            else
            {
                Vector2 targetPosition = (Vector2)stream.ReceiveNext();
                float targetRotation = (float)stream.ReceiveNext();
                Vector2 targetVelocity = (Vector2)stream.ReceiveNext();

                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));
                Vector2 newPosition = targetPosition + targetVelocity * lag;
                playerRigidbody.position = Vector2.Lerp(playerRigidbody.position, newPosition, Time.deltaTime * lagInterpolationSpeed);
                playerRigidbody.velocity = Vector2.Lerp(playerRigidbody.velocity, targetVelocity, Time.deltaTime * lagInterpolationSpeed);
            }
        }

        #endregion
    }
}
