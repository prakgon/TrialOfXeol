using System;
using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;

public class RestoreStaminaPlayer : MonoBehaviour
{
    public int staminaRestore = 25;
    
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
            var canRestore = playerStats.RestoreStamina(staminaRestore);
            if (canRestore)
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

    private void OnTriggerExit(Collider other)
    {
        destroyFX.Stop();
    }
}
