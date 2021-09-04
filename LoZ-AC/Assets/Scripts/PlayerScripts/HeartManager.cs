using System;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerScripts
{
    public class HeartManager : MonoBehaviour
    {
        [Header("Components")]
        public Image[] hearts;
        public Sprite fullHeart;
        public Sprite halfFullHeart;
        public Sprite emptyHeart;
        public FloatValue heartContainers;
        public FloatValue playerCurrentHealth;


        // Start is called before the first frame update
        private void Start()
        {
            InitHearts();
            UpdateHearts();
        }

        private void Update()
        {
            InitHearts();
            UpdateHearts();
        }

        /// <summary>
        /// Initialize hearts.
        /// </summary>
        private void InitHearts()
        {
            for (var i = 0; i < heartContainers.initialValue; i++)
            {
                hearts[i].gameObject.SetActive(true);
            }
        }
        
        /// <summary>
        /// Updates the hearts.
        /// </summary>
        public void UpdateHearts()
        {
            var tempHealth = playerCurrentHealth.runtimeValue / 2;
            
            for (var i = 0; i < heartContainers.initialValue; i++)
            {
                if (i <= tempHealth - 1)
                {
                    //Full Heart
                    hearts[i].sprite = fullHeart;
                }else if (i >= tempHealth)
                {
                    //empty Heart
                    hearts[i].sprite = emptyHeart;
                }
                else
                {
                    //half full heart
                    hearts[i].sprite = halfFullHeart;
                }
            }
        }
    }
}
