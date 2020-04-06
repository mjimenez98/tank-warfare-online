using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.TankWarfareOnline
{
    public class PlayerShooting : MonoBehaviourPunCallbacks
    {
        #region Properties


        public GameObject bulletPrefab;
        public GameObject cylinder;


        public AudioSource cannonSFX;


        #endregion


        #region MonoBehaviour Callbacks


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
