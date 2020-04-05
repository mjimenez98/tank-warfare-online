using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.TankWarfareOnline
{
    public class Wall : MonoBehaviourPun
    {
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

                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
}
