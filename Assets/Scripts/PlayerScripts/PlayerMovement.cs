using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Cinemachine;
using Photon.Pun;
using UnityEngine;
using static Helpers.Literals;
using Helpers;
using PlayerScripts;
using UnityEngine.InputSystem;


namespace TOX
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviourPunCallbacks, IPunObservable, IMediatorUser
    {
        /*This values are not being used, in runTime the editor gets the values configured
        in the inspector of the script attached to the player*/
        [Header("Player")] [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 5f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 7f;

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
        private PlayerInputHandler _input;
        private PlayerInput _playerInput;
        private PlayerController _playerController;
        private GameObject _mainCamera;
        private GameObject[] players;
        private GameObject opponent;
        private PlayerMediator _med;
        private PlayerAnimatorController _animController;

        private const float _threshold = 0.01f;
        private Vector3 _targetDirection;

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
                _input = GetComponent<PlayerInputHandler>();
                _playerInput = GetComponent<PlayerInput>();
                _playerInput.enabled = true;
                GameObject followCamera = Instantiate(_followCameraPrefab);
                followCamera.GetComponent<CinemachineVirtualCamera>().Follow = transform.GetChild(0).transform;
            }

            _playerController = GetComponent<PlayerController>();
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<PlayerInputHandler>();
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        #region Photon Methods

        public bool CheckPhotonView()
        {
            return (photonView.IsMine == false && PhotonNetwork.IsConnected == true);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        }

        #endregion


        #region Ground Check

        public void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);
            _animController.SetParameter(AnimatorParameters.Grounded, Grounded);
        }

        #endregion


        #region Camera Rotation

        public void CameraRotation()
        {
            if (!LockCameraPosition)
            {
                switch (_input.targetLock)
                {
                    case true:
                        _cinemachineTargetYaw = gameObject.transform.rotation.eulerAngles.y;
                        break;
                    default:
                        _cinemachineTargetYaw += _input.look.x * Time.deltaTime;
                        break;
                }

                _cinemachineTargetPitch += _input.look.y * Time.deltaTime;
            }

            _cinemachineTargetYaw = HelperFunctions.ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = HelperFunctions.ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        #endregion


        #region Movement

        public void HandlePlayerLocomotion()
        {
            try
            {
                if (_playerController.isInteracting)
                {
                    // Applying gravity when interacting e.g. Rolling Animation.
                    ControllerMove(new Vector3(0, 0, 0), 0);
                }
                else
                {
                    HandleMovement();
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        private void ControllerMove(Vector3 targetDirection, float speed)
        {
            _controller.Move(targetDirection * (speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        }

        public void HandleRollingAndSprinting()
        {
            if (_animController.GetBool(AnimatorParameters.IsInteracting)) return;
            try
            {
                if (!_input.rollFlag) return;
                if (_input.moveAmount > 0)
                {
                    _animController.SetParameter(AnimatorParameters.Horizontal, _input.move.x);
                    _animController.SetParameter(AnimatorParameters.Vertical, _input.move.y);
                    _animController.PlayTargetAnimation(AnimatorStates.Roll, true, 1);
                    _targetDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(_targetDirection);
                    transform.rotation = rollRotation;
                }
                else
                {
                    _animController.PlayTargetAnimation(AnimatorStates.BackStep, true, 1);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        #endregion

        private void HandleMovement()
        {
            float targetSpeed;
            if (_input.sprintFlag && _playerController.isSprinting && _input.moveAmount > 0.5)
            {
                targetSpeed = SprintSpeed;
                _playerController.isSprinting = true;
            }
            else
            {
                targetSpeed = MoveSpeed * _input.moveAmount;
                _playerController.isSprinting = false;
            }

            if (_input.move == Vector2.zero)
            {
                targetSpeed = 0.0f;
                _playerController.isSprinting = false;
            }

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;

            //
            float inputMagnitude = _input.move.magnitude;
            /*_speed = targetSpeed;*/
//

            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);

            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            if (_input.move != Vector2.zero)
            {
                TransformRotation(inputDirection, RotationSmoothTime);
            }

            TransformRotation(inputDirection, RotationSmoothTime);

            _targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // update animator if using character
            ControllerMove(_targetDirection, _speed);
            if (_animController.HasAnimator)
            {
                _animController.SetParameter(AnimatorParameters.Speed, _animationBlend);
                _animController.SetParameter(AnimatorParameters.MotionSpeed, inputMagnitude);
            }
        }


        #region Player Rotation

        private void TransformRotation(Vector3 inputDirection, float rotationSmoothTime)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              _mainCamera.transform.eulerAngles.y;
            switch (_input.targetLock)
            {
                case true:
                {
                    if (opponent != null)
                    {
                        Vector3 targetPosition = new Vector3(opponent.transform.position.x, transform.position.y,
                            opponent.transform.position.z);
                        transform.LookAt(targetPosition);
                    }

                    break;
                }
                default:
                {
                    if (_input.move != Vector2.zero && !_input.targetLock)
                    {
                        var rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation,
                            ref _rotationVelocity,
                            rotationSmoothTime);
                        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                    }

                    break;
                }
            }
        }

        #endregion


        #region Jump and Gravity

        public void JumpAndGravity()
        {
            if (Grounded)
            {
                _fallTimeoutDelta = FallTimeout;

                if (_animController.HasAnimator)
                {
                    _animController.SetParameter(AnimatorParameters.Jump, false);
                    _animController.SetParameter(AnimatorParameters.FreeFall, false);
                }

                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                try
                {
                    if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                    {
                        if (_animController.CurrentAnimatorState == AnimatorStates.IdleWalkRunBlend &&
                            !_animController.IsInTransition() && !_playerController.isInteracting)
                        {
                            _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                            if (_animController.HasAnimator)
                            {
                                _animController.SetParameter(AnimatorParameters.Jump, true);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                _jumpTimeoutDelta = JumpTimeout;

                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    if (_animController.HasAnimator)
                    {
                        _animController.SetParameter(AnimatorParameters.FreeFall, true);
                    }
                }

                _input.jump = false;
            }

            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        #endregion


        #region Helpers

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
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        #endregion

        public void ToggleTargetLock()
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                if (player != gameObject)
                {
                    opponent = player;
                }
            }

            _input.targetLock = !_input.targetLock;
        }

        public void ConfigureMediator(PlayerMediator med)
        {
            _med = med;
            _animController = _med.PlayerAnimatorController;
        }
    }
}