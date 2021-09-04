using ObjectsScripts;
using UnityEngine;

namespace ScriptableObjects
{
    [System.Serializable]
    public class DropItem
    {
        public Item drop;
        public int dropChance;
    }
    
    [CreateAssetMenu]
    public class DropItems : ScriptableObject
    {
        public DropItem[] loots;

        public Item LootDrop()
        {
            var number = 0;
            var percentage = Random.Range(0, 100);

            foreach (var t in loots)
            {
                number += t.dropChance;
                if (percentage <= number)
                {
                    return t.drop;
                }
            }
            return null;
        }
    }
}
