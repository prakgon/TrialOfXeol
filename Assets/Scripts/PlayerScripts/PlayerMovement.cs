using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine;
using static Helpers.Literals;
using Helpers;
using PlayerScripts;
using UnityEngine.InputSystem;
using VisualFX;


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

        [Tooltip("Roll Stamina Costs")] [SerializeField]
        private float rollStaminaCost = 10.0f;

        [Tooltip("Jump Stamina Costs")] [SerializeField]
        private float jumpStaminaCost = 5.0f;

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

        /*[Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;*/

        public Transform currentLockOnTarget;
        [SerializeField] 
        private List<PlayerController> availableTargets = new List<PlayerController>();
        public Transform nearestLockOnTarget;
        public Transform leftLockTarget;
        public Transform rightLockTarget;
        [Tooltip("Lock On Camera radius")] 
        public float LockOnRadius = 26.0f;
        [Tooltip("Maximum Lock On Camera 3radius")]
        public float maximumLockOnDistance = 30.0f;

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
        private PlayerStats _playerStats;
        private GameObject _mainCamera;
        private PlayerAnimatorController _animController;
        private PlayerMediator _playerMediator;
        private PlayerEffectsManager _playerEffectsManager;

        private const float _zero = 0.0f;
        private const float _thousand = 1000f;
        private Vector3 _targetDirection;
        private bool spawnMoveFX = true;

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

            _playerStats = GetComponent<PlayerStats>();
            _playerEffectsManager = GetComponent<PlayerEffectsManager>();
        }


        private void Start()
        {
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
            if (_input.lockOnFlag == false || currentLockOnTarget == null)
            {
                _cinemachineTargetYaw += _input.look.x * Time.deltaTime;

                /*_mainCamera.transform.position += new Vector3(0, 0, -10);*/

                _cinemachineTargetPitch += _input.look.y * Time.deltaTime;
                
                _cinemachineTargetYaw =
                    HelperFunctions.ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
                _cinemachineTargetPitch = HelperFunctions.ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
                CinemachineCameraTarget.transform.rotation = Quaternion.Euler(
                    _cinemachineTargetPitch + CameraAngleOverride,
                    _cinemachineTargetYaw, 0.0f);
            }
            else
            {
                _cinemachineTargetYaw = gameObject.transform.rotation.eulerAngles.y;

                float velocity = 0;

                Vector3 dir = currentLockOnTarget.position - _mainCamera.transform.position;
                dir.Normalize();
                dir.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(dir);
                CinemachineCameraTarget.transform.rotation = targetRotation;

                dir = currentLockOnTarget.position - _mainCamera.transform.position;
                dir.Normalize();

                targetRotation = Quaternion.LookRotation(dir);
                Vector3 eulerAngle = targetRotation.eulerAngles;
                eulerAngle.y = 0;
                _mainCamera.transform.localEulerAngles = eulerAngle;
            }
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
            if (_animController.GetBool(AnimatorParameters.IsInteracting))
                return;

            if (_playerStats.CurrentStamina <= 0)
                return;

            try
            {
                if (!_input.rollFlag) return;
                if (_input.moveAmount > 0)
                {
                    _animController.SetParameter(AnimatorParameters.Horizontal, _input.move.x);
                    _animController.SetParameter(AnimatorParameters.Vertical, _input.move.y);

                    _animController.PlayTargetAnimation(
                        _input.lockOnFlag ? AnimatorStates.Rolls : AnimatorStates.Roll, true, 1);

                    if (!_input.lockOnFlag)
                    {
                        _targetDirection.y = 0;
                        Quaternion rollRotation = Quaternion.LookRotation(_targetDirection);
                        transform.rotation = rollRotation;
                    }
                }
                else
                {
                    _animController.PlayTargetAnimation(AnimatorStates.BackStep, true, 1);
                }

                _playerStats.DrainStamina(rollStaminaCost);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        private void HandleMovement()
        {
            float targetSpeed;
            if (_input.sprintFlag && _playerController.isSprinting && _input.moveAmount > 0.5 &&
                _playerStats.CurrentStamina > 0)
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

            if (_playerController.isSprinting)
            {
                _playerStats.DrainStamina(5 * Time.deltaTime);
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
                _speed = Mathf.Round(_speed * _thousand) / _thousand;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);

            Vector3 inputDirection = new Vector3(_input.move.x, _zero, _input.move.y).normalized;

            if (_input.move != Vector2.zero)
            {
                TransformRotation(inputDirection, RotationSmoothTime);
            }

            //TransformRotation(inputDirection, RotationSmoothTime);

            _targetDirection = Quaternion.Euler(_zero, _targetRotation, _zero) * Vector3.forward;

            // update animator if using character
            ControllerMove(_targetDirection, _speed);
        }

        public void HandleMoveAnimation()
        {
            if (!_playerController.isInteracting)
            {
                if (!_animController.HasAnimator) return;
                _animController.SetParameter(AnimatorParameters.Speed, _animationBlend);
                _animController.SetParameter(AnimatorParameters.MotionSpeed, _input.move.magnitude);
                _animController.SetParameter(AnimatorParameters.Horizontal,
                    Mathf.Lerp(_animController.GetFloat(AnimatorParameters.Horizontal), _input.move.x, 0.1f));
                _animController.SetParameter(AnimatorParameters.Vertical,
                    Mathf.Lerp(_animController.GetFloat(AnimatorParameters.Vertical), _input.move.y, 0.1f));
                /*_animController.SetParameter(AnimatorParameters.Horizontal, _input.move.x);
                _animController.SetParameter(AnimatorParameters.Vertical, _input.move.y);*/
            }
        }

        public void HandleMoveEffects()
        {
            if (Grounded && _speed > _zero && spawnMoveFX && _playerStats.CurrentHealth > 0)
            {
                _playerEffectsManager.PlayMoveFX();
                StartCoroutine(ResetMoveFX());
            }
        }

        private IEnumerator ResetMoveFX()
        {
            spawnMoveFX = false;
            yield return new WaitForSeconds(_speed > MoveSpeed ? 0.2f : 0.15f);
            spawnMoveFX = true;
        }

        #endregion


        #region Player Rotation

        private void TransformRotation(Vector3 inputDirection, float rotationSmoothTime)
        {
            /*_targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              _mainCamera.transform.eulerAngles.y;
            if (_input.move != Vector2.zero)
            {
                var rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation,
                    ref _rotationVelocity,
                    rotationSmoothTime);
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }*/
            
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              _mainCamera.transform.eulerAngles.y;
            /*if (_input.move != Vector2.zero)*/
            switch (_input.lockOnFlag)
            {
                case true:
                {
                    if (currentLockOnTarget != null)
                    {
                        Vector3 targetPosition = new Vector3(currentLockOnTarget.transform.position.x, transform.position.y,
                            currentLockOnTarget.transform.position.z);
                        transform.LookAt(targetPosition);
                    }

                    break;
                }
                default:
                {
                    if (_input.move != Vector2.zero && !_input.lockOnFlag)
                    {
                        var rotationAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation,
                            ref _rotationVelocity,
                            rotationSmoothTime);
                        transform.rotation = Quaternion.Euler(0.0f, rotationAngle, 0.0f);
                    }

                    break;
                }
                var rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation,
                    ref _rotationVelocity,
                    rotationSmoothTime);
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
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
                    if (_playerStats.CurrentStamina <= 0)
                        return;


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

                            _playerStats.DrainStamina(jumpStaminaCost);
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
        
        public void StrafeTransition(bool strafe)
        {
            _animController.SetParameter(AnimatorParameters.IsLocking, strafe && currentLockOnTarget != null);
        }

        public void HandleLockOn()
        {
            float shortestDistance = Mathf.Infinity;
            float shortestDistanceOfLeftTarget = Mathf.Infinity;
            float shortestDistanceOfRightTarget = Mathf.Infinity;

            Collider[] colliders = Physics.OverlapSphere(transform.position, LockOnRadius);

            foreach (var t in colliders)
            {
                PlayerController otherPlayer = t.GetComponent<PlayerController>();

                if (otherPlayer != null)
                {
                    Vector3 lockTargetDirection = otherPlayer.transform.position - transform.position;
                    float distanceFromTarget = Vector3.Distance(transform.position, otherPlayer.transform.position);
                    float viewableAngle = Vector3.Angle(lockTargetDirection, CinemachineCameraTarget.transform.forward);

                    if (otherPlayer.transform.root != transform.root
                        && viewableAngle is > -50 and < 50
                        && distanceFromTarget <= maximumLockOnDistance)
                    {
                        availableTargets.Add(otherPlayer);
                    }
                }
            }

            foreach (var t in availableTargets)
            {
                float distanceFromTarget = Vector3.Distance(transform.position, t.transform.position);

                if (distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOnTarget = t.lockOnTransform;
                }

                if (_input.lockOnFlag)
                {
                    Vector3 relativeEnemyPosition = currentLockOnTarget.InverseTransformPoint(t.transform.position);
                    float distanceFromLeftTarget = currentLockOnTarget.transform.position.x - t.transform.position.x;
                    float distanceFromRightTarget = currentLockOnTarget.transform.position.x + t.transform.position.x;

                    switch (relativeEnemyPosition.x)
                    {
                        case > 0 when distanceFromLeftTarget < shortestDistanceOfLeftTarget:
                            shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                            leftLockTarget = t.lockOnTransform;
                            break;
                        case < 0 when distanceFromRightTarget < shortestDistanceOfRightTarget:
                            shortestDistanceOfRightTarget = distanceFromRightTarget;
                            rightLockTarget = t.lockOnTransform;
                            break;
                    }

                    /*if (relativeEnemyPosition.x > 0 && distanceFromLeftTarget < shortestDistanceOfLeftTarget)
                    {
                        shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                        leftLockTarget = t.lockOnTransform;
                    }
                    
                    if (relativeEnemyPosition.x < 0 && distanceFromRightTarget < shortestDistanceOfRightTarget) 
                    {
                        shortestDistanceOfRightTarget = distanceFromRightTarget;
                        rightLockTarget = t.lockOnTransform;
                    }*/
                }
            }
        }
        
        public void ClearLockOnTargets()
        {
            availableTargets.Clear();
            nearestLockOnTarget = null;
            currentLockOnTarget = null;
        }

        public void ConfigureMediator(PlayerMediator med)
        {
            _playerMediator = med;
            _animController = med.PlayerAnimatorController;

            if (photonView.IsMine)
            {
                _input = med.PlayerInputHandler;
                _playerInput = med.PlayerInput;
                _playerInput.enabled = true;
                var followCamera = Instantiate(_followCameraPrefab);
                followCamera.GetComponent<CinemachineVirtualCamera>().Follow = transform.GetChild(0).transform;
            }

            _playerController = med.PlayerController;
            _controller = med.CharacterController;
            _input = med.PlayerInputHandler;
        }
        
    }
}