using DefaultNamespace;
using UnityEngine;

public class PlayerEffectsManager : CharacterFXManager
{
    // Reference on inspector
    [SerializeField] private ParticleSystem deathFX;
    [SerializeField] private ParticleSystem dashFX;

    private void Awake()
    {
        deathFX.Stop();
        dashFX.Stop();
    }

    public void PlayDeathFX()
    {
        var deathFXRotation = transform.rotation * Vector3.back;
        var particles = Instantiate(deathFX, transform.position + new Vector3(0, 1, 0),  Quaternion.LookRotation(deathFXRotation));
        Destroy(particles, particles.time);
        /*deathFX.Play();*/
    }

    public void PlayDashFX()
    {
        var dashTransform = transform.rotation * Vector3.back;
        var particles = Instantiate(dashFX, transform.position + new Vector3(0, 1, 0), Quaternion.LookRotation(dashTransform));
        Destroy(particles, particles.time);
    }
}