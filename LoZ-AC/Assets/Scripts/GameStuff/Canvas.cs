using UnityEngine;

namespace GameStuff
{
    public class Canvas : MonoBehaviour
    {
        public static Canvas instance;

        private void Awake()
        {
            DestroyGameObject();
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>    
        /// If gameobject already exist, Destroy it.
        /// </summary>
        private void DestroyGameObject()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }

        public void DestroyMe()
        {
            Destroy(this);
        }
    }
}
