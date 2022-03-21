using StarterAssets;
using PlayerScripts;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using TMPro;
using UIScripts;

namespace Helpers
{
    public class PlayerMediator : MonoBehaviour
    {
        [Header("MOVEMENT-related dependencies")]
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private StarterAssetsInputs _starterAssetsInputs;
        [Header("GUI-related dependencies")]
        [SerializeField] private StamineBar _stamineBar;
        [SerializeField] private SliderBar healthBar;
        [Header("PLAYER MECHANICS-related dependencies")]
        [SerializeField] private PlayerAnimatorController _playerAnimatorController;
        [SerializeField] private PlayerMechanics _playerMechanics;
        private Animator _an;
        [Header("DEBUG-related dependencies")]
        [SerializeField] private PlayerDataSO playerData;
        [SerializeField] private GameObject playerWeapon;
        [SerializeField] private SkinnedMeshRenderer playerMeshRenderer;
        [SerializeField] private TMP_Text playerTMPText;

        public PlayerMovement PlayerMovement { get => _playerMovement; set => _playerMovement = value; }
        public StarterAssetsInputs StarterAssetsInputs { get => _starterAssetsInputs; set => _starterAssetsInputs = value; }
        public StamineBar StamineBar { get => _stamineBar; set => _stamineBar = value; }
        public PlayerAnimatorController PlayerAnimatorController { get => _playerAnimatorController; set => _playerAnimatorController = value; }
        public PlayerMechanics PlayerMechanics { get => _playerMechanics; set => _playerMechanics = value; }
        public Animator An { get => _an; set => _an = value; }
        public TMP_Text PlayerTMPText { get => playerTMPText; set => playerTMPText = value; }
        public SkinnedMeshRenderer PlayerMeshRenderer { get => playerMeshRenderer; set => playerMeshRenderer = value; }
        public GameObject PlayerWeapon { get => playerWeapon; set => playerWeapon = value; }
        public PlayerDataSO PlayerData { get => playerData; set => playerData = value; }

        public SliderBar HealthBar
        {
            get => healthBar;
            set => healthBar = value;
        }

        private void Awake()
        {
            An = GetComponent<Animator>();
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