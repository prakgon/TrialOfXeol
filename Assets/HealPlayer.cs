using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using PlayerScripts;
using UnityEngine;

public class HealPlayer : MonoBehaviour
{
    public int heal = 25;

    [SerializeField] private ParticleSystem loopFX;
    [SerializeField] private ParticleSystem destroyFX;

    private void Awake()
    {
        destroyFX.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        var playerStats = other.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            var canHeal = playerStats.HealPlayer(heal);
            if (canHeal)
            {
                PlayFX();

                var destroy = GetComponent<PropDestroyer>();
                if (destroy != null)
                {
                    loopFX.Stop();
                    Debug.Log("DESTROY");
                    destroy.DestroyProp(gameObject);
                }
            }
        }
    }

    [PunRPC]
    private void PlayFX(bool isRemote = false)
    {
        if (!isRemote)
        {
            PhotonView.Get(gameObject).RPC("PlayFX", RpcTarget.Others, true);
        }

        var fxTransform = destroyFX.transform;
        var particles = Instantiate(destroyFX, fxTransform.position, fxTransform.rotation);
        Destroy(particles.gameObject, destroyFX.main.duration + 1f);
    }
}