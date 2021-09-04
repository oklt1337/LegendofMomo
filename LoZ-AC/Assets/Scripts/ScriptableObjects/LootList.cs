using System.Collections.Generic;
using ObjectsScripts;
using UnityEngine;

namespace ScriptableObjects
{
    [System.Serializable]
    public class Loot
    {
        public Powerup drop;
        public int dropChance;
    }
    
    [CreateAssetMenu]
    public class LootList : ScriptableObject
    {
        public Loot[] loots;

        public Powerup LootDrop()
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
