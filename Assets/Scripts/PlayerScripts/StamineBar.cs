using System.Collections;
using UnityEngine;
using Helpers;
using Photon.Pun;
using static Helpers.Literals;

namespace PlayerScripts
{
    /*public class StamineBar : MonoBehaviourPunCallbacks, IPunObservable, IMediatorUser
    {
        [Header("Parameters")]
        [SerializeField] private float _coolDown;//usar esto cooldwon routien
        [SerializeField] private float _processSpeed;
        [SerializeField] private float _returnSpeed;
        [SerializeField] private SliderBarStates _currentState;
        [Space(10)]
        [Header("References")]
        private StarterAssetsInputs _input;
        private PlayerMovement _playerMovement;
        [SerializeField] private Transform _fullBarPoint;
        [SerializeField] private Transform _emptyBarPoint;
        private PlayerMediator _med;

        private void Start()
        {
            _playerMovement = _med.PlayerMovement;
            _input = _med.StarterAssetsInputs;
        }
        public void ConfigureMediator(PlayerMediator med)
        {
            _med = med;
        }

        private void ProcessBar(float speed, Transform targetPos)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPos.position, step);
        }

        private void Update()
        {
            DecideBehaviour();
            StamineBarBehaviour();
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
            if (PlayerIsSprintingCheck() && !_currentState.Equals(SliderBarStates.Cooldown))
            {
                _currentState = SliderBarStates.Processing;
                if (GetCurrentPos() == _emptyBarPoint.position.x)
                {
                    _currentState = SliderBarStates.Cooldown;
                    _playerMovement.isSprinting = false;
                    StartCoroutine(CooldownRoutine());
                }
            }

            if (!PlayerIsSprintingCheck())
            {
                _playerMovement.isSprinting = true;
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
            yield return new WaitForSeconds(_coolDown);
            if (transform.position.x > _emptyBarPoint.position.x)
            {
                _playerMovement.isSprinting = true;
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        }
    }*/
}