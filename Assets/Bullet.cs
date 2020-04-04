using System.Collections;
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
            Invoke("ActivateCollider", 1);
        }

        private void ActivateCollider()
        {
            gameObject.GetComponent<SphereCollider>().enabled = true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            // Destroy if hits an object
            if (!photonView.IsMine)
                return;

            PhotonNetwork.Destroy(gameObject);
        }


        #endregion
    }
}
