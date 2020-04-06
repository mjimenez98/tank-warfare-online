using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.TankWarfareOnline
{
    public class Invincibility : Powerup
    {
        #region Properties


        private readonly static float timer = 10.0f;

        public Material material;


        #endregion


        #region MonoBehaviour Callbacks


        private new void Awake()
        {
            base.Awake();
        }


        #endregion


        #region Public Methods


        public Color GetColor()
        {
            return material.color;
        }


        public void PhotonNetworkDestroy()
        {
            photonView.RPC("Destroy", RpcTarget.All);
        }


        #endregion


        #region Static Methods


        public static float GetTimer()
        {
            return timer;
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
