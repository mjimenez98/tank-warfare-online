using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.TankWarfareOnline
{
    public class PlayerManager : MonoBehaviourPunCallbacks
    {
        #region Properties


        [Tooltip("The local player instance. Use this to know if the local " +
            "player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        public float speed;
        public float rotationSpeed;

        private enum PlayerPowerups { Lightweight, Invincibility, None }
        private PlayerPowerups powerupInPossession = PlayerPowerups.None;

        [Tooltip("Speed multiplier determined by power-ups and external events")]
        private float speedMultiplier = 1.0f;

        [Tooltip("Indicates for how longer the power-up will be active")]
        private float powerupTimer = 0.0f;

        public ParticleSystem powerupParticles;


        #endregion


        #region MonoBehaviour Callbacks


        private void Start()
        {
            if (!photonView.IsMine)
                return;

            // #Important
            // Used in GameManager.cs: we keep track of the localPlayer instance
            // to prevent instantiation when levels are synchronized
            PlayerManager.LocalPlayerInstance = this.gameObject;

            // #Critical
            // We flag as don't destroy on load so that instance survives
            // level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
        }

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

            // Get the horizontal and vertical axis.
            // By default they are mapped to the arrow keys.
            // The value is in the range -1 to 1
            float translationZ = Input.GetAxis("Vertical") * speed * speedMultiplier *
                Time.deltaTime;
            float rotation = Input.GetAxis("Horizontal") * rotationSpeed * speedMultiplier *
                Time.deltaTime;

            // Move translation along the object's z-axis
            transform.Translate(0, 0, translationZ);

            // Rotate around our y-axis
            transform.Rotate(0, rotation, 0);
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
