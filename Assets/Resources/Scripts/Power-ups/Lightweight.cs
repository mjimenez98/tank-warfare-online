using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.TankWarfareOnline
{
    public class Lightweight : Powerup
    {
        #region Properties

        private readonly static float timer = 10.0f;
        private readonly static float speedMultiplier = 3.0f;


        #endregion


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


        #region Static Methods


        public static float GetSpeedMultiplier()
        {
            return speedMultiplier;
        }

        public static float GetTimer()
        {
            return timer;
        }


        #endregion
    }
}
