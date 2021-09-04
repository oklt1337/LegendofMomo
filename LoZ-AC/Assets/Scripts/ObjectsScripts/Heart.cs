using System;
using ScriptableObjects;
using UnityEngine;

namespace ObjectsScripts
{
    public class Heart : Powerup
    {
        [Header("Heart Stats")]
        public FloatValue playerHealth;
        public FloatValue heartContainers;
        public int healthIncrease;
        public int maxHeartContainers;
        
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || other.isTrigger ||
                !(heartContainers.initialValue < maxHeartContainers)) return;

            playerHealth.runtimeValue += healthIncrease * 2f;
            heartContainers.initialValue += healthIncrease;

            powerUpSignal.Raise();
            Destroy(gameObject);
        }
    }
}
