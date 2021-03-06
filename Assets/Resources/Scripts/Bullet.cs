﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.TankWarfareOnline
{
    public class Bullet : MonoBehaviourPunCallbacks
    {
        #region Properties


        public float speed;


        #endregion


        #region Private Methods


        void Start()
        {
            // If enabled, collision is detected with player, thus destroying the object
            gameObject.GetComponent<SphereCollider>().enabled = false;

            Rigidbody r = GetComponent<Rigidbody>();
            r.velocity = transform.forward * speed;

            // Reactivate collider
            Invoke("ActivateCollider", 0.5f);
        }

        private void ActivateCollider()
        {
            gameObject.GetComponent<SphereCollider>().enabled = true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!photonView.IsMine)
                return;

            //Destroy if hits an object
            PhotonNetwork.Destroy(gameObject);
        }


        #endregion
    }
}
