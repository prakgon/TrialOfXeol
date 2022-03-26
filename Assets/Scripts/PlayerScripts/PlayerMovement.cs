using Cinemachine;
using Photon.Pun;
using UnityEngine;
using static Helpers.Literals;
using Helpers;
using InputSystem;
using PlayerScripts;


namespace TOX
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : PlayerBase, IPunObservable, IMediatorUser
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

        [Header("Roll Parameters")] [SerializeField]
        private float _rollSpeedFactor = 4;


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
        private InputHandler _inputHandler;
        private GameObject _mainCamera;
        private PlayerMediator _med;

        private const float _threshold = 0.01f;
        private Vector3 _targetDirection;

        public Vector3 TargetDirection
        {
            get => _targetDirection;
            set => _targetDirection = value;
        }

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
                _inputHandler = GetComponent<InputHandler>();
                _inputHandler.enabled = true;
                GameObject followCamera = Instantiate(_followCameraPrefab);
                followCamera.GetComponent<CinemachineVirtualCamera>().Follow = transform.GetChild(0).transform;
            }

            _controller = GetComponent<CharacterController>();
            _inputHandler = GetComponent<InputHandler>();
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }


        private void Update()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            float delta = Time.deltaTime;
            _inputHandler.TickInput(delta);

            AnimationStateCheck();
            HandleInteractions();

            JumpAndGravity();
            GroundedCheck();

            HandlePlayerLocomotion();
            HandleRollingAndSprinting(delta);

            _inputHandler.isInteracting = _animController.GetBool(AnimatorParameters.isInteracting);
            _inputHandler.rollFlag = false;
            _inputHandler.sprintFlag = false;
        }

        private void LateUpdate()
        {
            CameraRotation();
        }


        #region Ground Check

        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);
            _animController.SetParameter(AnimatorParameters.Grounded, Grounded);
        }

        #endregion


        #region Camera Rotation

        private void CameraRotation()
        {
            if (_inputHandler.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                _cinemachineTargetYaw += _inputHandler.look.x * Time.deltaTime;
                _cinemachineTargetPitch += _inputHandler.look.y * Time.deltaTime;
            }

            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        #endregion


        #region Movement

        private void HandlePlayerLocomotion()
        {
            if (isInteracting)
            {
                // Applying gravity when interacting e.g. Rolling Animation.
                ControllerMove(new Vector3(0, 0, 0), 0);
            }
            else
            {
                HandleMovement();
            }
        }

        private void ControllerMove(Vector3 targetDirection, float speed)
        {
            _controller.Move(targetDirection * (speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        }

        // TODO: Move to Animator Controller
        private void HandleRollingAndSprinting(float delta)
        {
            if (_animController.GetBool(AnimatorParameters.isInteracting)) return;

            if (_inputHandler.rollFlag)
            {
                _animController.SetParameter(AnimatorParameters.Horizontal, 1);
                _animController.SetParameter(AnimatorParameters.Horizontal, 1);
                /*_animController.SetParameter(AnimatorParameters.Horizontal, _inputHandler.move.x);
                _animController.SetParameter(AnimatorParameters.Vertical, _inputHandler.move.y);*/
                if (_inputHandler.moveAmount > 0)
                {
                    //_animController.PlayTargetAnimation(AnimatorStates.Roll, true, (int)PlayerLayers.Rolls);
                    _animController.PlayTargetAnimation(AnimatorStates.Roll, true, (int) PlayerLayers.BaseLayer);
                    //_animController.PlayTargetAnimation(AnimatorStates.Rolls, true, (int) PlayerLayers.BaseLayer);
                    _targetDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(_targetDirection);
                    transform.rotation = rollRotation;
                }
                else
                {
                    //_animController.PlayTargetAnimation(AnimatorStates.BackStep, true, (int)PlayerLayers.Rolls);
                    _animController.PlayTargetAnimation(AnimatorStates.BackStep, true, (int)PlayerLayers.BaseLayer);
                    //_animController.PlayTargetAnimation(AnimatorStates.Rolls, true, (int) PlayerLayers.BaseLayer);
                }
            }
        }

        private void HandleMovement()
        {
            float targetSpeed;
            if (_inputHandler.sprintFlag && isSprinting)
            {
                targetSpeed = SprintSpeed;
            }
            else
            {
                targetSpeed = MoveSpeed * _inputHandler.moveAmount;
            }

            if (_inputHandler.move == Vector2.zero)
            {
                targetSpeed = 0.0f;
            }

            float inputMagnitude = _inputHandler.move.magnitude;
            _speed = targetSpeed;

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            Vector3 inputDirection = new Vector3(_inputHandler.move.x, 0.0f, _inputHandler.move.y).normalized;
            if (_inputHandler.move != Vector2.zero)
            {
                TransformRotation(inputDirection, RotationSmoothTime);
            }

            _targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
            // update animator if using character
            ControllerMove(_targetDirection, _speed);
            if (_animController.HasAnimator)
            {
                _animController.SetParameter(AnimatorParameters.Speed, _animationBlend);
                _animController.SetParameter(AnimatorParameters.MotionSpeed, inputMagnitude);
            }
        }

        #endregion


        #region Player Rotation

        private void TransformRotation(Vector3 inputDirection, float rotationSmoothTime)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              _mainCamera.transform.eulerAngles.y;
            var rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        #endregion


        #region Jump and Gravity

        private void JumpAndGravity()
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

                if (_inputHandler.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    if (_animController.CurrentAnimatorState == AnimatorStates.IdleWalkRunBlend &&
                        !_animController.IsInTransition() && !isInteracting)
                    {
                        _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                        if (_animController.HasAnimator)
                        {
                            _animController.SetParameter(AnimatorParameters.Jump, true);
                        }
                    }
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

                _inputHandler.jump = false;
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