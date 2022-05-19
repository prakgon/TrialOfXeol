using System;
using System.Collections;
using System.Threading.Tasks;
using Helpers;
using Photon.Pun;
using UnityEngine;

public class PropDestroyer : MonoBehaviour
{
    public void DestroyProp(GameObject prop)
    {
        PhotonView.Get(prop).TransferOwnership(PhotonNetwork.LocalPlayer);
        AsyncNetworkDestroy(PhotonView.Get(prop));
    }

    public void DestroyProp(GameObject prop, float time)
    {
        GetComponent<Collider>().enabled = false;
        PhotonView.Get(prop).TransferOwnership(PhotonNetwork.LocalPlayer);
        StartCoroutine(DelayedNetworkDestroy(prop, time));
    }

    private async Task AsyncNetworkDestroy(PhotonView view, int interval = 50, int timeout = 1000)
    {
        while (!view.Owner.IsLocal || timeout < 0)
        {
            timeout -= interval;
            await Task.Delay(interval);
        }

        PhotonNetwork.Destroy(gameObject);
    }

    IEnumerator DelayedNetworkDestroy(GameObject prop, float time)
    {
        yield return new WaitForSeconds(time);
        AsyncNetworkDestroy(PhotonView.Get(prop));
    }
}