using Unity.Mathematics;
using UnityEngine;

public class OHGUIController : MonoBehaviour
{
    private Transform _localCameraTransform;

    private void Start()
    {
        _localCameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        var position = _localCameraTransform.position;
        transform.LookAt(new Vector3(position.x, transform.position.y,
            position.z));
        
        transform.rotation = Quaternion.LookRotation(transform.rotation * Vector3.back);
        // Bug solved: flip the UI to face the camera correctly
    }
}