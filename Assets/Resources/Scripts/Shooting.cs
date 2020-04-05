using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.TankWarfareOnline
{
    public class Shooting : MonoBehaviourPunCallbacks
    {
        #region Properties


        public GameObject bulletPrefab;
        public GameObject cylinder;


        public AudioSource cannonSFX;


        #endregion


        #region Private Methods


        // Update is called once per frame
        void Update()
        {
            if (!photonView.IsMine)
                return;

            if (Input.GetButtonDown("Jump"))
            {
                Debug.Log("Firing tank");

                PhotonNetwork.Instantiate(
                    "Prefabs/" + bulletPrefab.name,
                    cylinder.transform.position,
                    transform.rotation);

                photonView.RPC("PlayCannonSFX", RpcTarget.All);
            }
        }

        [PunRPC]
        void PlayCannonSFX()
        {
            Debug.Log("Playing cannon shot SFX");

            cannonSFX.Play();
        }


        #endregion
    }

}
