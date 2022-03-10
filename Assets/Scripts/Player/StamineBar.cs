using StarterAssets;
using System.Collections;
using UnityEngine;
using Helpers;
using Photon.Pun;
namespace TOX
{
    public class StamineBar : MonoBehaviourPunCallbacks, IPunObservable, IInjectorUser
    {
        [Header("Parameters")]
        [SerializeField] private float _coolDown;
        [SerializeField] private float _processSpeed;
        [SerializeField] private float _returnSpeed;
        [SerializeField] private SliderBarStates _currentState;
        [Space(10)]
        [Header("References")]
        private StarterAssetsInputs _input;
        private PlayerMovement _playerMovement;
        [SerializeField] private Transform _fullBarPoint;
        [SerializeField] private Transform _emptyBarPoint;
        private DependencyInjector _inj;

        private void Start()
        {
            GetDependencies();
        }
        public void ConfigureInjector(DependencyInjector inj)
        {
            _inj = inj;
            Debug.Log(_inj);
        }

        private void ProcessBar(float speed, Transform targetPos)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPos.position, step);
        }

        private void Update()
        {
            GetDependencies();

            DecideBehaviour();
            StamineBarBehaviour();
            Debug.Log("tt");
        }

        private bool PlayerIsSprintingCheck()
        {
            return _input.sprint;
        }

        private float GetCurrentPos()
        {
            return transform.position.x;
        }

        private void DecideBehaviour()
        {
            Debug.Log(_playerMovement.enabled);
            if (PlayerIsSprintingCheck() && !_currentState.Equals(SliderBarStates.Cooldown))
            {
                _currentState = SliderBarStates.Processing;
                if (GetCurrentPos() == _emptyBarPoint.position.x)
                {
                    _currentState = SliderBarStates.Cooldown;
                    _playerMovement.CanSprint = false;
                    StartCoroutine(CooldownRoutine());
                }
            }

            if (!PlayerIsSprintingCheck())
            {
                _playerMovement.CanSprint = true;
                _currentState = SliderBarStates.Returning;
                if (GetCurrentPos() == _fullBarPoint.position.x)
                {
                    _currentState = SliderBarStates.Idle;
                }
            }
        }

        private void StamineBarBehaviour()
        {
            switch (_currentState)
            {
                case SliderBarStates.Idle:
                    break;
                case SliderBarStates.Processing:
                    ProcessBar(_processSpeed, _emptyBarPoint);
                    break;
                case SliderBarStates.Returning:
                    ProcessBar(_returnSpeed, _fullBarPoint);
                    break;
                case SliderBarStates.Cooldown:
                    break;
            }
        }

        IEnumerator CooldownRoutine()
        {
            yield return new WaitForSeconds(1);
            if (transform.position.x > _emptyBarPoint.position.x)
            {
                _playerMovement.CanSprint = true;
            }
        }

        public void GetDependencies()
        {
            _input = _inj.GetDependency<StarterAssetsInputs>();
            _playerMovement = _inj.GetDependency<PlayerMovement>();
        }
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        }
    }

    public enum SliderBarStates
    {
        Idle,
        Processing,
        Returning,
        Cooldown
    }
}