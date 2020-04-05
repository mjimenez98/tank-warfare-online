using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.TankWarfareOnline
{
    public class Invincibility : Powerup
    {
        #region MonoBehaviour Callbacks


        private new void Awake()
        {
            base.Awake();
        }

        private new void OnCollisionEnter(Collision collision)
        {
            base.OnCollisionEnter(collision);
        }


        #endregion
    }
}
