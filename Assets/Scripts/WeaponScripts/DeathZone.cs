using PlayerScripts;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public int damage = 100;

    private void OnTriggerStay(Collider other)
    {
        var playerStats = other.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            playerStats.TakeDamage(damage);
            // playerStats.PlayBloodVFX(other);
        }
    }
}