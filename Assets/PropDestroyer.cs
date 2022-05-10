using Helpers;
using Photon.Pun;
using UnityEngine;

public class PropDestroyer : MonoBehaviour
{
    public void DestroyProp(GameObject prop)
    {
        PhotonNetwork.Destroy(prop);
    }

    public void DestroyProp(GameObject prop, float time)
    {
        GetComponent<Collider>().enabled = false;
        HelperFunctions.ExecuteAfterDelay(() => PhotonNetwork.Destroy(prop), time);
    }
}