using UnityEngine;

namespace PlayerScripts
{
    public class InventoryDisplay : MonoBehaviour
    {
        public GameObject inventoryDisplay;
        public void ActivateInventoryDisplay()
        {
            inventoryDisplay.SetActive(!inventoryDisplay.activeSelf);
        }
    }
}