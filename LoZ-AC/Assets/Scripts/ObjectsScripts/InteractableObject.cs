using ScriptableObjects;
using UnityEngine;

namespace ObjectsScripts
{
    public class InteractableObject : MonoBehaviour
    {
        [Header("Interactable Stats")]
        public Signal context;
        public bool playerInRage;

        public virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || other.isTrigger) return;

            context.Raise();
            playerInRage = true;
        }

        public virtual void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || other.isTrigger) return;
            
            context.Raise();
            playerInRage = false;
        }
    }
}
