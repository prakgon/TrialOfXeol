using System;
using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;

public class RestoreStaminaPlayer : MonoBehaviour
{
    public int staminaRestore = 100;

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
                PlayFX(other.transform);

                var destroy = GetComponent<PropDestroyer>();
                if (destroy != null)
                {
                    loopFX.Stop();
                    destroy.DestroyProp(gameObject, 0.5f);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        destroyFX.Stop();
    }

    private void PlayFX(Transform player)
    {
        var fxTransform = destroyFX.transform;
        var particles = Instantiate(destroyFX, player.position, player.rotation, player.transform);
        particles.gameObject.SetActive(true);
        particles.gameObject.AddComponent<StaminaPowerUp>();
        Destroy(particles.gameObject, destroyFX.main.duration + 1f);
    }
}