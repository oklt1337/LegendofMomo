using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class Inventory : ScriptableObject
    {
        public Item currentItem;
        public List<Item> items = new List<Item>();
        public int numberOfKeys;
        public int coins;

        public void AddItem(Item itemToAdd)
        {
            if (itemToAdd.isKey)
            {
                numberOfKeys++;
            }
            else
            {
                if (!items.Contains(itemToAdd))
                {
                    itemToAdd.amount++;
                    items.Add(itemToAdd);
                }
                else
                {
                    itemToAdd.amount++;
                }
            }
        }
    }
}
