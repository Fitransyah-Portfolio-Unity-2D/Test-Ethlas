using UnityEngine;
using Spine.Unity;
using Shooter.Player;
using Photon.Pun;
using Shooter.Combat;

namespace Shooter.Core
{
    public class SpineAnimHandler : MonoBehaviourPun
    {
        enum MovementState
        {
            Idle,
            Run,
            JumpUp,
            JumpDown
        }
        [SerializeField] float velocityXThreshold = 3f;

        SkeletonAnimation skeletonAnimation;
        Mover mover;
        Gunner gunner;

        MovementState movementState;

        bool isJumping;
        private void Awake()
        {
            SkeletonAnimation[] skeletonAnimations = GetComponentsInChildren<SkeletonAnimation>();
            skeletonAnimation = skeletonAnimations[0];
            mover = GetComponent<Mover>();
            gunner = GetComponent<Gunner>();
        }

        private void OnEnable()
        {
            mover.OnMoving += OnMovingAction;
            mover.OnStopMoving += OnStopMovingAction;
            mover.OnJumpUp += OnJumpUpAction;
            mover.OnJumpDown += OnJumpDownAction;
            mover.OnLanding += OnLandingAction;

            gunner.OnAttackTriggered += OnAttackTriggeredAction;


        }

        private void OnAttackTriggeredAction()
        {
            photonView.RPC("SetFire", RpcTarget.All);
        }

        private void OnDisable()
        {
            mover.OnMoving -= OnMovingAction;
            mover.OnStopMoving -= OnStopMovingAction;
            mover.OnJumpUp -= OnJumpUpAction;
            mover.OnJumpDown -= OnJumpDownAction;
            mover.OnLanding -= OnLandingAction;

        }

        private void OnMovingAction()
        {
            movementState = MovementState.Run;
            UpdateAnim();
        }

        private void OnStopMovingAction()
        {
            movementState = MovementState.Idle;
            UpdateAnim();
        }

        private void OnJumpUpAction()
        {
            movementState = MovementState.JumpUp;
            UpdateAnim();
        }

        private void OnJumpDownAction()
        {
            movementState = MovementState.JumpDown;
            UpdateAnim();
        }

        private void OnLandingAction()
        {
            Invoke("CheckMovingAfterLanding", 0.25f);
        }

        private void CheckMovingAfterLanding()
        {
            float velocityX = mover.GetComponent<Rigidbody2D>().velocity.x;

            if (velocityX == 0f)
            {
                movementState = MovementState.Idle;
                UpdateAnim();
            }

            if (velocityX > velocityXThreshold || velocityX < -velocityXThreshold)
            {
                movementState = MovementState.Run;
                UpdateAnim();
            }
        }

        private void UpdateAnim()
        {
            switch (movementState) 
            {
                case MovementState.JumpDown:
                    photonView.RPC("SetJumpDown", RpcTarget.All);
                    break;
                case MovementState.JumpUp:
                    photonView.RPC("SetJumpUp", RpcTarget.All);
                    break;
                case MovementState.Run:
                    photonView.RPC("SetRun", RpcTarget.All);
                    break;
                case MovementState.Idle:
                    photonView.RPC("SetIdle", RpcTarget.All);
                    break;
            }
        }

        [PunRPC]
        void SetIdle()
        {
            if (skeletonAnimation != null)
            {
                skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
            }
        }


        [PunRPC]
        private void SetRun()
        {
            if (skeletonAnimation != null)
            {
                skeletonAnimation.AnimationState.SetAnimation(0, "Run", true);
            }
        }


        [PunRPC]
        private void SetJumpUp()
        {
            if (skeletonAnimation != null)
            {
                 skeletonAnimation.AnimationState.SetAnimation(0, "JumpUp", true);
            }
        }

        [PunRPC]
        private void SetJumpDown()
        {
            if (skeletonAnimation != null)
            {

                skeletonAnimation.AnimationState.SetAnimation(0, "JumpDown", true);
            }
        }

        [PunRPC]
        void SetFire()
        {
            if (skeletonAnimation != null)
            {
                skeletonAnimation.AnimationState.SetAnimation(0, "Fire", false);
                OnLandingAction();
                
            }
        }
    }
}

