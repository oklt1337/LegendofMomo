using System;
using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace GameStuff
{
    public class CoinTextManager : MonoBehaviour
    {
        [Header("Components")]
        public Inventory playerInventory;
        public TextMeshProUGUI coinDisplay;

        private void Update()
        {
            UpdateCoinCount();
        }

        public void UpdateCoinCount()
        {
            coinDisplay.text = "" + playerInventory.coins;
        }
    }
}
