using Cinemachine;
using Helpers;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class FreeSpectatorMovement : MonoBehaviourPunCallbacks
{
    private CharacterController _controller;
    private Vector3 _move;
    private Vector2 _look;
    private float _cinemachineTargetYaw, _cinemachineTargetPitch;
    private Transform _cameraTargetTransform;
    [SerializeField] private GameObject _followCameraPrefab;
    public GameObject CinemachineCameraTarget;
    public GameObject followCamera;
    public float speed;
    public float TopClamp = 70.0f;
    public float BottomClamp = -30.0f;
    
    private void Start()
    {
        if (photonView.IsMine)
        {
            var playerInput = GetComponent<PlayerInput>();
            playerInput.enabled = true;
            followCamera = Instantiate(_followCameraPrefab);
            followCamera.GetComponent<CinemachineVirtualCamera>().Follow = transform.GetChild(0).transform;
        }
        _cameraTargetTransform = CinemachineCameraTarget.transform;
        _controller = GetComponent<CharacterController>();
    }
    
    private void Update()
    {
        var directedMove = _cameraTargetTransform.right.normalized * _move.x + _cameraTargetTransform.up.normalized * _move.y + _cameraTargetTransform.forward.normalized * _move.z;
        _controller.Move(directedMove*Time.deltaTime*speed);
        CameraRotation();
    }

    public void OnMove(InputValue value) {
        _move = value.Get<Vector3>();
    }

    public void OnLook(InputValue value)
    {
        _look = value.Get<Vector2>();
    }

    private void CameraRotation()
    {
        _cinemachineTargetYaw += _look.x * Time.deltaTime;
        _cinemachineTargetPitch += _look.y * Time.deltaTime;

        _cinemachineTargetYaw = HelperFunctions.ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = HelperFunctions.ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);
    }

}
