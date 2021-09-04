using ScriptableObjects;
using UnityEngine;

namespace ObjectsScripts
{
    public class Coin : Powerup
    {
        public Inventory playerInventory;

        private void Start()
        {
            powerUpSignal.Raise();
        }
        
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || other.isTrigger) return;
            
            playerInventory.coins += 1;
            powerUpSignal.Raise();
            
            Destroy(gameObject);
        }
    }
}
