using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class PlayerStats : ScriptableObject
    {
        public float playerHealth;
        public float playerHeartContainers;
        public float speed;
        public float damage;
        public int playerInventory;
    }
}
