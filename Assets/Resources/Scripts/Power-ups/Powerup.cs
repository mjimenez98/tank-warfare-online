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


        #region Protected Methods


        protected void Awake()
        {
            if (!photonView.IsMine)
                return;

            DontDestroyOnLoad(this.gameObject);
        }

        protected float GetTimer()
        {
            return timer;
        }


        #endregion
    }
}
