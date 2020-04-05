using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.TankWarfareOnline
{
    public class Powerup : MonoBehaviourPunCallbacks
    {
        #region Properties


        public float timer;


        #endregion


        #region MonoBehaviour Callbacks


        protected void Awake()
        {
            if (!photonView.IsMine)
                return;

            DontDestroyOnLoad(this.gameObject);
        }

        protected void OnCollisionEnter(Collision collision)
        {
            if (!photonView.IsMine)
                return;

            Debug.Log("COLLISION");

            if (collision.gameObject.GetComponentInParent<PlayerManager>() != null)
            {
                Debug.Log("Collision with Player. Destroying Power-up");

                PhotonNetwork.Destroy(this.gameObject);
            }
        }


        #endregion


        #region Protected Methods


        protected float GetTimer()
        {
            return timer;
        }


        #endregion
    }
}
