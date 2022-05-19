using System;
using PlayerScripts;
using UnityEngine;

public class StaminaPowerUp : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;

    private void Awake()
    {
        playerStats = transform.parent.GetComponent<PlayerStats>();
    }

    private void Update()
    {
        playerStats.IncreaseStamina();
    }
}