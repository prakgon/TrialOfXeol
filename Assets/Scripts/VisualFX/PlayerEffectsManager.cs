
using DefaultNamespace;
using UnityEngine;

public class PlayerEffectsManager : CharacterFXManager
{
    // Reference on inspector
    [SerializeField] private ParticleSystem deathFX;

    private void Awake()
    {
        deathFX.Stop();
    }

    public void PlayDeathFX()
    {
        var particles = Instantiate(deathFX, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        /*particles.transform.rotation = transform.rotation*/
        Destroy(particles, particles.time);
        /*deathFX.Play();*/
    }
}
