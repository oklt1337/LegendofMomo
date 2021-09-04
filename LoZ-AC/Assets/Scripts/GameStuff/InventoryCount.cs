using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace GameStuff
{
    public class InventoryCount : MonoBehaviour
    {
        public Inventory inventory;
        public Item item;
        public TextMeshProUGUI text;

        // Update is called once per frame
        private void Update()
        {
            DisplayInventory();
        }

        private void DisplayInventory()
        {
            text.text = inventory.items.Contains(item) ? item.amount.ToString() : "0";
        }
    }
}
