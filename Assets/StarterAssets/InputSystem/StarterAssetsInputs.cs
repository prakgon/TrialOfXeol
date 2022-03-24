using Helpers;
using PlayerScripts;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour, IMediatorUser
    {
        [Header("Character Input Values")] public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        //public bool roll;
        public bool lightAttack;
        public bool heavyAttack;
        public bool block;
        public bool targetLock = false;
        private PlayerMediator _med;
        private PlayerMechanics _playerMechanics;
        private PlayerMovement _playerMovement;

        [Header("Movement Settings")] public bool analogMovement;

#if !UNITY_IOS || !UNITY_ANDROID
        [Header("Mouse Cursor Settings")] public bool cursorLocked = true;
        public bool cursorInputForLook = true;
#endif

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook) LookInput(value.Get<Vector2>());
        }

        public void OnMove(InputValue value) => MoveInput(value.Get<Vector2>());
        public void OnJump(InputValue value) => JumpInput(value.isPressed);
        public void OnSprint(InputValue value) => SprintInput(value.isPressed);
        public void OnRoll(InputValue value) => RollInput(value.isPressed);
        public void OnLightAttack(InputValue value) => LightAttackInput(value.isPressed);
        public void OnHeavyAttack(InputValue value) => HeavyAttackInput(value.isPressed);
        public void OnBlock(InputValue value) => BlockInput(value.isPressed);
        public void OnLockTarget(InputValue value) => LockTargetInput();

#else
	// old input sys if we do decide to have it (most likely wont)...
#endif
        public void MoveInput(Vector2 newMoveDirection) => move = newMoveDirection;
        public void LookInput(Vector2 newLookDirection) => look = newLookDirection;
        public void JumpInput(bool newJumpState) => jump = newJumpState;
        public void SprintInput(bool newSprintState) => sprint = newSprintState;
        public void RollInput(bool newRollState) => _playerMechanics.Roll(newRollState);
        public void LightAttackInput(bool newLightAttackState) => _playerMechanics.LightAttack(newLightAttackState);
        public void HeavyAttackInput(bool newHeavyAttackState) => heavyAttack = newHeavyAttackState;
        public void BlockInput(bool newBlockState) => block = newBlockState;
        public void LockTargetInput() => _playerMovement.ToggleTargetLock();

#if !UNITY_IOS || !UNITY_ANDROID
        private void OnApplicationFocus(bool hasFocus) => SetCursorState(cursorLocked);
        private void SetCursorState(bool newState) =>
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;

        public void ConfigureMediator(PlayerMediator med)
        {
            _med = med;
            _playerMechanics = _med.PlayerMechanics;
            _playerMovement = _med.PlayerMovement;
        }
#endif
    }
}