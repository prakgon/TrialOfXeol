using UnityEngine;

public class PropDestroyer : MonoBehaviour
{
    public void DestroyProp(GameObject prop) => Destroy(prop);
    public void DestroyProp(GameObject prop, float time) => Destroy(prop, time);
}