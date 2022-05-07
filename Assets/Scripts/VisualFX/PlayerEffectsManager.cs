using System;
using DefaultNamespace;
using Helpers;
using UnityEngine;

public class PlayerEffectsManager : CharacterFXManager
{
    // Reference on inspector
    [SerializeField] private ParticleSystem deathFX;
    [SerializeField] private ParticleSystem currentDashFX;
    [SerializeField] public ParticleSystem healFX;

    [Serializable]
    public struct DashFX
    {
        public ParticleSystem dashFX;
        public Literals.Colors color;
    }

    [SerializeField] private DashFX[] dashFXs;

    private void Awake()
    {
        deathFX.Stop();
        currentDashFX.Stop();
        healFX.Stop();
        foreach (var t in dashFXs)
        {
            t.dashFX.Stop();
        }
    }

    public void PlayDeathFX()
    {
        var deathFXRotation = transform.rotation * Vector3.back;
        var particles = Instantiate(deathFX, transform.position + new Vector3(0, 1, 0),
            Quaternion.LookRotation(deathFXRotation));
        Destroy(particles, particles.time);
        /*deathFX.Play();*/
    }

    public void PlayDashFX()
    {
        var dashTransform = transform.rotation * Vector3.back;
        var particles = Instantiate(currentDashFX, transform.position + new Vector3(0, 1, 0),
            Quaternion.LookRotation(dashTransform));
        Destroy(particles, particles.time);
    }

    public void PlayHealFX()
    {
        var healTransform = transform.rotation * Vector3.back;
        var particles = Instantiate(healFX, transform.position + new Vector3(0, 1, 0),
            Quaternion.LookRotation(healTransform));
        Destroy(particles, particles.time);
    }

    public void SetCurrentDashFX(Literals.Colors dashFXColor)
    {
        foreach (var t in dashFXs)
        {
            if (t.color != dashFXColor) continue;
            currentDashFX = t.dashFX;
            return;
        }
    }
}