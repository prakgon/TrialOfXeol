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
    }
}