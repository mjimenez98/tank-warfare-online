using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.TankWarfareOnline
{
    public class ZombieManager : MonoBehaviourPunCallbacks
    {
        #region Properties


        public float movementSpeed;
        public float rotationSpeed;

        private bool isShooting = false;
        private bool isMoving = true;
        private bool isRotating = false;

        private Vector3 lastRotationPosition;


        #endregion


        #region Public Methods


        public void SetShooting(bool state)
        {
            isShooting = state;
        }

        public bool GetShooting()
        {
            return isShooting;
        }


        #endregion


        #region MonoBehaviour Callbacks


        void Awake()
        {
            if (!photonView.IsMine)
                return;

            DontDestroyOnLoad(this.gameObject);
        }

        private void FixedUpdate()
        {
            if (!photonView.IsMine)
                return;

            if (!isShooting)
            {
                if (isMoving && !isRotating)
                {
                    transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
                    lastRotationPosition = transform.eulerAngles;
                }
                else if (!isMoving && isRotating)
                {
                    if (Mathf.Abs(lastRotationPosition.y - transform.eulerAngles.y) >= 179.9f &&
                        Mathf.Abs(lastRotationPosition.y - transform.eulerAngles.y) <= 180.1f)
                    {
                        Debug.Log("Stop Rotation");

                        isRotating = false;
                        isMoving = true;
                    }
                    else
                    {
                        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!photonView.IsMine)
                return;

            if (!isRotating && !other.gameObject.name.Contains("Bullet"))
            {
                isRotating = true;
                isMoving = false;
            }
        }


        #endregion
    }
}
