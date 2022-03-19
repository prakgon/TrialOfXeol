#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif
using System;
using Cinemachine;
using Photon.Pun;
using UnityEngine;
using static Helpers.Literals;
using Helpers;
using PlayerScripts;

/* Note: animations are called via the controller for both the character and capsule using animator null checks
*/

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class PlayerMovement : PlayerBase, IPunObservable, IMediatorUser
    {
        [Header("Player")] [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 10f; //This values are not being used, in runTime the editor gets the values configured in the inspector of the script attached to the player

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 7.335f;

        [Tooltip("How fast the character turns to face movement direction")] [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        [Space(10)] [Tooltip("The height the player can jump")]
        public float JumpHeight = 1f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")] public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        [Header("Roll Parameters")]
        [SerializeField] private float _rollSpeedFactor = 4;
        //private int _animIDGrounded;
        //private int _animIDJump;
        //private int _animIDFreeFall;
        //private int _animIDMotionSpeed;
        //private int _animIDRoll;
        //private int _animIDLightAttack;
        //private int _animIDSpeed;
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;
        private PlayerMediator _med;
        private const float _threshold = 0.01f;
        private Vector3 _targetDirection;
        public Vector3 TargetDirection { get => _targetDirection; set => _targetDirection = value; }
        public float Speed { get => _speed; set => _speed = value; }
        public static GameObject LocalPlayerInstance;
        [SerializeField] private GameObject _followCameraPrefab;

        private void Awake()
        {
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag(Tags.MainCamera.ToString());
            }

            if (photonView.IsMine)
            {
                LocalPlayerInstance = gameObject;
            }
            DontDestroyOnLoad(gameObject);
        }


        private void Start()
        {
            if (photonView.IsMine)
            {
                GameObject followCamera = Instantiate(_followCameraPrefab);
                followCamera.GetComponent<CinemachineVirtualCamera>().Follow = transform.GetChild(0).transform;
            }

            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }
            CanJumpCheck();
            CanRotateCheck();
            CanMoveCheck();
            JumpAndGravity();
            GroundedCheck();
            Move();
            ActionHandler();
        }

        private void ActionHandler()
        {
            if(_animController.CompareAnimState(PlayerStates.Roll.ToString()))
            {
                ControllerMove(transform.forward, _rollSpeedFactor);
            }
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
            _animController.ChangeState(PlayerParameters.Grounded, Grounded);
        }

        private void CameraRotation()
        {
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                _cinemachineTargetYaw += _input.look.x * Time.deltaTime;
                _cinemachineTargetPitch += _input.look.y * Time.deltaTime;
            }

            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
        }

        private void Move()
        {
            float targetSpeed;
            if (_input.sprint && CanSprint)
            {
                targetSpeed = SprintSpeed;
            }

            else
            {
                targetSpeed = MoveSpeed;
            }

            if (_input.move == Vector2.zero)
            {
                targetSpeed = 0.0f;
                _animController.CurrentPlayerState = PlayerStates.IdleWalkRunBlend;
            }
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;
            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                Speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed /** inputMagnitude*/,Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                Speed = Mathf.Round(Speed * 1000f) / 1000f;
            }

            else
            {
                Speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);

            ApplyMovement(inputMagnitude);
            HandlePlayerRotation();
        }

        private void ApplyMovement(float inputMagnitude)
        {
            if (!_canMove) return;
            ControllerMove(TargetDirection, Speed);
            UpdateAnimator(inputMagnitude);
        }

        public void HandlePlayerRotation()
        {
            if (!_canRotate) return;
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y);

            if (_input.move != Vector2.zero)
            {
                TransformRotation(inputDirection);
            }
            TargetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        }

        private void UpdateAnimator(float inputMagnitude)
        {
            _animController.ChangeState(PlayerParameters.Speed, _animationBlend);
            _animController.ChangeState(PlayerParameters.MotionSpeed, inputMagnitude);
        }

        public void ControllerMove(Vector3 targetDirection, float speed) //working to apply the normal movement to run and sprint
        {
            
            _controller.Move(targetDirection * (speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        }

        private void TransformRotation(Vector3 inputDirection)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            var rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator 
                _animController.ChangeState(PlayerParameters.Jump, false);
                _animController.ChangeState(PlayerParameters.FreeFall, false);

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if(_canJump)
                {
                    if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                    {
                        if (_input.move != Vector2.zero && !_animController.IsTransition())
                        {
                            // the square root of H * -2 * G = how much velocity needed to reach desired height
                            _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                            // update animator
                            _animController.ChangeState(PlayerParameters.Jump, true);
                        }
                    }
                }
                

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }

            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    _animController.ChangeState(PlayerParameters.FreeFall, true);
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        }

        public void ConfigureMediator(PlayerMediator med)
        {
            _med = med;
            _animController = _med.PlayerAnimatorController;
        }
    }
}