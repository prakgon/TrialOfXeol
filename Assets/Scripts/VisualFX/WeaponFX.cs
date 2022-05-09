using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFX : MonoBehaviour
{
    [Header("Weapon FX")] public ParticleSystem normalWeaponTrail;

    public ParticleSystem glowWeapon;
    // fire weapon trail
    // dark weapon trail
    // lightning weapon trail

    private void Awake()
    {
        normalWeaponTrail.Stop();
        glowWeapon.Stop();
    }

    public void PlayWeaponTrailFX()
    {
        try
        {
            normalWeaponTrail.Stop();

            if (normalWeaponTrail.isStopped)
            {
                normalWeaponTrail.Play();
            }
            else
            {
                normalWeaponTrail.Play();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void PlayWeaponGlowFX()
    {
        try
        {
            glowWeapon.Stop();

            if (glowWeapon.isStopped)
            {
                glowWeapon.Play();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}