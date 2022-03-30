using PlayerScripts;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using TMPro;
using TOX;
using UIScripts;
using UnityEngine.InputSystem;

namespace Helpers
{
    public class PlayerMediator : MonoBehaviour
    {
        [Header("MOVEMENT-related dependencies")]
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private CharacterController _characterController;
        [Header("GUI-related dependencies")]
        //[SerializeField] private StamineBar _stamineBar;
        [SerializeField] private SliderBar _healthBar;
        [Header("PLAYER MECHANICS-related dependencies")]
        [SerializeField] private PlayerAnimatorController _playerAnimatorController;
        [SerializeField] private PlayerCombatManager _playerCombatManager;
        [SerializeField] private PlayerInventory _playerInventory;
        [SerializeField] private PlayerInputHandler _playerInputHandler;
        [SerializeField] private Animator _an;
        [Header("DEBUG-related dependencies")]
        [SerializeField] private PlayerDataSO playerData;
        [SerializeField] private GameObject playerWeapon;
        [SerializeField] private SkinnedMeshRenderer playerMeshRenderer;
        [SerializeField] private TMP_Text playerTMPText;

        public PlayerMovement PlayerMovement => _playerMovement;
        public PlayerAnimatorController PlayerAnimatorController => _playerAnimatorController;
        public Animator An => _an;
        public TMP_Text PlayerTMPText => playerTMPText;
        public SkinnedMeshRenderer PlayerMeshRenderer => playerMeshRenderer;
        public GameObject PlayerWeapon => playerWeapon;
        public PlayerDataSO PlayerData => playerData;
        public PlayerCombatManager PlayerCombatManager => _playerCombatManager;
        public PlayerInventory PlayerInventory => _playerInventory;
        public PlayerController PlayerController => _playerController;
        public PlayerInput PlayerInput => _playerInput;
        public SliderBar HealthBar => _healthBar;
        public PlayerInputHandler PlayerInputHandler => _playerInputHandler;
        public CharacterController CharacterController => _characterController;

        private void Awake()
        {
            var mediatorUsers = GetComponentsInChildren<MonoBehaviour>().OfType<IMediatorUser>();
            var mediatorUsers2 = GetComponentsInChildren<MonoBehaviourPunCallbacks>().OfType<IMediatorUser>();

            foreach (var user in mediatorUsers)
            {
                user.ConfigureMediator(this);
            }

            foreach (var user in mediatorUsers2)
            {
                user.ConfigureMediator(this);
            }
        }
    }
}