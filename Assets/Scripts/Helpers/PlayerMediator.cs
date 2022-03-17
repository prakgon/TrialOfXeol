using StarterAssets;
using PlayerScripts;
using UnityEngine;
using System.Linq;
using Photon.Pun;

namespace Helpers
{
    public class PlayerMediator : MonoBehaviour
    {
        [Header("MOVEMENT-related dependencies")]
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private StarterAssetsInputs _starterAssetsInputs;
        
        [Header("GUI-related dependencies")]
        [SerializeField] private StamineBar _stamineBar;

        [Header("PLAYER MECHANICS-related dependencies")]
        [SerializeField] private PlayerAnimatorController _playerAnimatorController;
        [SerializeField] private PlayerMechanics _playerMechanics;

        public PlayerMovement PlayerMovement { get => _playerMovement; set => _playerMovement = value; }
        public StarterAssetsInputs StarterAssetsInputs { get => _starterAssetsInputs; set => _starterAssetsInputs = value; }
        public StamineBar StamineBar { get => _stamineBar; set => _stamineBar = value; }
        public PlayerAnimatorController PlayerAnimatorController { get => _playerAnimatorController; set => _playerAnimatorController = value; }
        public PlayerMechanics PlayerMechanics { get => _playerMechanics; set => _playerMechanics = value; }

        private void Awake()
        {
            var mediatorUsers = FindObjectsOfType<MonoBehaviour>().OfType<IMediatorUser>();
            var mediatorUsers2 = FindObjectsOfType<MonoBehaviourPunCallbacks>().OfType<IMediatorUser>();

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