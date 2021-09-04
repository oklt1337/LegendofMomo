using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class Item : ScriptableObject
    {
        public Sprite itemSprite;
        public string itemDescription;
        public bool isKey;
        public bool onlyOnce;
        public bool consumable;
        public int amount;
    }
}
