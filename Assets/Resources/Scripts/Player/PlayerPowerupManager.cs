using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.TankWarfareOnline
{
    public class PlayerPowerupManager : MonoBehaviourPunCallbacks
    {
        #region Properties


        private enum PlayerPowerups { Lightweight, Invincibility, None }
        private PlayerPowerups powerupInPossession = PlayerPowerups.None;

        [Tooltip("Speed multiplier determined by power-ups and external events")]
        private float speedMultiplier = 1.0f;

        [Tooltip("Indicates for how longer the power-up will be active")]
        private float powerupTimer = 0.0f;

        public ParticleSystem powerupParticles;


        #endregion


        #region MonoBehaviour Callbacks


        private void Update()
        {
            if (!photonView.IsMine)
                return;

            if (powerupInPossession != PlayerPowerups.None)
            {
                powerupTimer -= Time.deltaTime;

                if (powerupTimer < 0.0f)
                {
                    Debug.Log(gameObject.name + " has lost power-up");

                    powerupInPossession = PlayerPowerups.None;
                    speedMultiplier = 1.0f;
                    powerupTimer = 0.0f;

                    photonView.RPC("DeactivatePowerupParticles", RpcTarget.AllBuffered);
                }
            }

            GetComponent<PlayerManager>().SetSpeedMultiplier(speedMultiplier);
        }


        private void OnCollisionEnter(Collision collision)
        {
            if (!photonView.IsMine)
                return;

            // Power-ups
            if (collision.gameObject.name.Contains("Lightweight"))
            {
                Debug.Log(gameObject.name + " has acquired Lightweight");

                // Set values according to the power-up acquired
                powerupInPossession = PlayerPowerups.Lightweight;
                speedMultiplier = Lightweight.GetSpeedMultiplier();
                powerupTimer = Lightweight.GetTimer();

                // Get power-up from collision
                Lightweight lightweight = collision.gameObject.GetComponent<Lightweight>();
                Color lightweightColor = lightweight.GetColor();

                // Play particle effect to demonstrate player has acquired a power-up
                photonView.RPC("ActivatePowerupParticles", RpcTarget.AllBuffered,
                    lightweightColor.r,
                    lightweightColor.g,
                    lightweightColor.b,
                    lightweightColor.g);

                // Destroy power-up
                lightweight.PhotonNetworkDestroy();
            }
            else if (collision.gameObject.name.Contains("Invincibility"))
            {
                Debug.Log(gameObject.name + " has acquired Invincibility");

                // Set values according to the power-up acquired
                powerupInPossession = PlayerPowerups.Invincibility;
                powerupTimer = Invincibility.GetTimer();

                // Get power-up from collision
                Invincibility invincibility = collision.gameObject.GetComponent<Invincibility>();

                // Play particle effect to demonstrate player has acquired a power-up
                photonView.RPC("ActivatePowerupParticles", RpcTarget.All,
                    invincibility.GetColor().r,
                    invincibility.GetColor().g,
                    invincibility.GetColor().b,
                    invincibility.GetColor().a);

                // Destroy power-up
                invincibility.PhotonNetworkDestroy();
            }

            if (collision.gameObject.name.Contains("Bullet"))
            {
                if (powerupInPossession != PlayerPowerups.Invincibility)
                {
                    Debug.Log(gameObject.name + " has died");

                    PhotonNetwork.Destroy(this.gameObject);
                }
            }
        }


        #endregion


        #region RPCs


        [PunRPC]
        void ActivatePowerupParticles(float r, float g, float b, float a)
        {
            ParticleSystem.MainModule main = powerupParticles.main;
            main.startColor = new Color(r, g, b, a);

            powerupParticles.Play();
        }

        [PunRPC]
        void DeactivatePowerupParticles()
        {
            powerupParticles.Stop();
        }


        #endregion
    }
}
