using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.TankWarfareOnline
{
    public class Wall : MonoBehaviourPun
    {
        #region MonoBehaviour Callbacks


        void Awake()
        {
            if (!photonView.IsMine)
                return;

            DontDestroyOnLoad(this.gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!photonView.IsMine)
                return;

            if (collision.gameObject.name.Contains("Bullet"))
            {
                Debug.Log("Destroying wall");

                photonView.RPC("Destroy", RpcTarget.All);
            }
        }


        #endregion


        #region RPCs


        [PunRPC]
        void Destroy()
        {
            if (!photonView.IsMine)
                return;

            PhotonNetwork.Destroy(this.gameObject);
        }


        #endregion
    }
}
