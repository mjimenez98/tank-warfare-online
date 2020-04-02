using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.TankWarfareOnline
{
    public class Spawn : MonoBehaviourPunCallbacks, IPunObservable
    {
        private Vector3 position;
        public bool isAvailable;

        #region Methods

        void Awake()
        {
            if (!photonView.IsMine)
                return;

            DontDestroyOnLoad(this.gameObject);

            position = transform.position;
            isAvailable = true;
        }

        public Vector3 GetPosition()
        {
            return position;
        }

        public void SetIsAvailable(bool available)
        {
            isAvailable = available;
        }

        public bool GetIsAvailable()
        {
            return isAvailable;
        }


        #endregion


        #region IPunObservable implementation


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                Debug.Log("Updating Spawn");

                // We own this player: send the others our data
                stream.SendNext(isAvailable);
            }
            else
            {
                Debug.Log("Reading Spawn");

                // Network player, receive data
                this.isAvailable = (bool)stream.ReceiveNext();
            }
        }


        #endregion
    }
}
