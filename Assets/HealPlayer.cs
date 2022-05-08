using System;
using System.Collections;
using System.Collections.Generic;
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
                destroyFX.Play();
                
                var destroy = GetComponent<PropDestroyer>(); 
                if (destroy != null)
                {
                    destroy.DestroyProp(gameObject);
                }
                //loopFX.Stop();
                //Destroy(gameObject);
            }
        }
    
    }
}
