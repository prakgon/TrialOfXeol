using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputHandler : MonoBehaviour
    {
        // Basic inputs
        public Vector2 move;
        public float moveAmount;
        public Vector2 look;

        // Combat mechanics inputs
        public bool jump;
        public bool rollFlag;
        public bool sprintFlag;
        public float rollInputTimer;
        public bool isInteracting;

        private InputActionSystem _inputActions;
        //private CameraHandler _cameraHandler;

        // Input Handlers
        private Vector2 _movementInput;
        private Vector2 _cameraInput;

        public bool b_Input;

        private void Awake()
        {
            /*
        _cameraHandler = CameraHandler.Singleton;
    */
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            /*if (_cameraHandler != null)
        {
            _cameraHandler.FollowTarget(delta);
            _cameraHandler.HandleCameraRotation(delta, mouseX, mouseY);
        }*/
        }

        public void OnEnable()
        {
            if (_inputActions == null)
            {
                _inputActions = new InputActionSystem();
                /*_inputActions.Player.Move.performed +=
                    inputActions => _movementInput = inputActions.ReadValue<Vector2>();
                _inputActions.Player.Look.performed +=
                    inputActions => _cameraInput = inputActions.ReadValue<Vector2>();*/
            }

            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            HandleBasicsMovementInputs();
            HandleRollingAndSprintingInputs(delta);
        }

        private void HandleBasicsMovementInputs()
        {
            move = _movementInput.normalized;
            moveAmount = move.magnitude;
            look = _cameraInput;
            //jump = _inputActions.Player.Jump.phase == InputActionPhase.Started;

        }

        private void HandleRollingAndSprintingInputs(float delta)
        {
            //b_Input = _inputActions.Player.Roll.phase == InputActionPhase.Started;

            if (b_Input)
            {
                rollInputTimer += delta;
                sprintFlag = true;
            }
            else
            {
                if (rollInputTimer is > 0 and < 0.5f)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }

                rollInputTimer = 0;
            }
        }
    }
}