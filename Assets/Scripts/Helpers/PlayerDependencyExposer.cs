using UnityEngine;
using StarterAssets;

namespace Helpers
{
    public class PlayerDependencyExposer : DependencyExposer, ISceneController
    {
        [Header("MOVEMENT-related dependencies")]
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private StarterAssetsInputs _starterAssetsInputs;
        //[Header("STAMINE BAR-related dependencies")]
        //[SerializeField] private StamineBar _stamineBar;
    }
}