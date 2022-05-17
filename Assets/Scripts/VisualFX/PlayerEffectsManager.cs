using System;
using DefaultNamespace;
using Helpers;
using PlayerScripts;
using UnityEngine;

namespace VisualFX
{
    public class PlayerEffectsManager : CharacterFXManager
    {
        private PlayerController _playerController;

        // Reference on inspector
        [SerializeField] private ParticleSystem deathFX;
        [SerializeField] private ParticleSystem currentDashFX;
        [SerializeField] public ParticleSystem healFX;
        [SerializeField] public ParticleSystem moveFX;

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
            moveFX.Stop();
            foreach (var t in dashFXs)
            {
                t.dashFX.Stop();
            }

            _playerController = GetComponent<PlayerController>();
        }

        public void PlayDeathFX()
        {
            var deathFXRotation = transform.rotation * Vector3.up;
            var particles = Instantiate(deathFX, transform.position + new Vector3(0, 1, 0),
                deathFX.transform.rotation);
            particles.Play();
            Destroy(particles.gameObject, deathFX.main.duration + 1f);
        }

        public void PlayDashFX()
        {
            var dashTransform = transform.rotation * Vector3.back;
            var particles = Instantiate(currentDashFX, transform.position + new Vector3(0, 1, 0),
                Quaternion.LookRotation(dashTransform));
            Destroy(particles.gameObject, currentDashFX.main.duration + 1f);
        }

        public void PlayHealFX()
        {
            var particles = Instantiate(healFX, transform.position,
                Quaternion.LookRotation(transform.rotation * Vector3.up), transform);
            Destroy(particles.gameObject, healFX.main.duration + 1f);
        }

        public void PlayMoveFX()
        {
            var moveTransform = transform.rotation * Vector3.back;
            var particles = Instantiate(moveFX, transform.position, Quaternion.LookRotation(moveTransform));
            Destroy(particles.gameObject, moveFX.main.duration + 1f);
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

        public void PlayMoveAudio()
        {
            if (!_playerController.isInteracting)
            {
                AudioManager.Instance.AtPoint(Literals.AudioType.Footstep, transform.position);
            }
        }

        public void PlayRollAudio()
        {
            AudioManager.Instance.AtPoint(Literals.AudioType.GroundHit, transform.position);
        }
    }
}