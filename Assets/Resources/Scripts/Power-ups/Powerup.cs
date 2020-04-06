using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.TankWarfareOnline
{
    public class Powerup : MonoBehaviourPunCallbacks
    {
        #region MonoBehaviour Callbacks


        protected void Awake()
        {
            if (!photonView.IsMine)
                return;

            DontDestroyOnLoad(this.gameObject);
        }


        #endregion
    }
}
