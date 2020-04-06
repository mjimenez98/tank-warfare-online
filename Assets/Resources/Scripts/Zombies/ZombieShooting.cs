using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.TankWarfareOnline
{
    public class ZombieShooting : MonoBehaviourPunCallbacks
    {
        #region Properties


        public float shootingCooldown;
        public float movingCooldown;

        public GameObject bulletPrefab;
        public GameObject cylinder;
        public ZombieManager zombieManager;

        public AudioSource cannonSFX;

        private float cooldownTimer = 0.0f;


        #endregion


        #region MonoBehaviour Callbacks


        private void Update()
        {
            if (!photonView.IsMine)
                return;

            RaycastHit hit;

            if (Physics.Raycast(cylinder.transform.position, transform.TransformDirection(Vector3.forward),
                out hit, Mathf.Infinity))
            {
                Debug.DrawRay(cylinder.transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);

                if (hit.collider.gameObject.tag.Contains("Player"))
                {
                    zombieManager.SetShooting(true);

                    Debug.Log("Found target");
                    Shoot();
                }
                else
                {
                    if (movingCooldown <= 0.0f)
                        zombieManager.SetShooting(false);
                }
            }

            movingCooldown -= Time.deltaTime;
        }


        #endregion


        #region Private Methods


        private void Shoot()
        {
            if (cooldownTimer < 0.0f)
            {
                Debug.Log("Firing zombie");


                PhotonNetwork.Instantiate(
                    "Prefabs/" + bulletPrefab.name,
                    cylinder.transform.position,
                    transform.rotation);

                photonView.RPC("PlayCannonSFX", RpcTarget.All);

                cooldownTimer = shootingCooldown;
                movingCooldown = 2.0f;
            }
            else
            {
                cooldownTimer -= Time.deltaTime;
            }
        }


        #endregion


        #region RPCs


        [PunRPC]
        void PlayCannonSFX()
        {
            Debug.Log("Playing cannon shot SFX");

            cannonSFX.Play();
        }


        #endregion
    }
}
