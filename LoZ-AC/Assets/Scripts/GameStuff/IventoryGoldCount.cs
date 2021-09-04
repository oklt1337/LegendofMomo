using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace GameStuff
{
    public class IventoryGoldCount : MonoBehaviour
    {
        public Inventory inventory;
        public TextMeshProUGUI text;
        
        // Update is called once per frame
        private void Update()
        {
            DisplayInventory();
        }

        private void DisplayInventory()
        {
            text.text = inventory.coins.ToString();
        }
    }
}
