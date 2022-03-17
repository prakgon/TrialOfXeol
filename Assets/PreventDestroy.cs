using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PreventDestroy : MonoBehaviourPunCallbacks, IPunObservable
{
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Dont Destroy on Load: " + name);
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
