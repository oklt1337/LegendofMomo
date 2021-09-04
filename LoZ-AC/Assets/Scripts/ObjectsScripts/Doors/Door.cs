using ScriptableObjects;
using UnityEngine;

namespace ObjectsScripts.Doors
{
    public enum DoorType
    {
       Key,
       Button,
       Kill,
       Closed
    }
    
    public class Door : InteractableObject
    {
        [Header("Door Stats")]
        public DoorType thisDoorType;
        public bool opened;
        
        [Header("Components")]
        public Inventory playerInventory;
        public SpriteRenderer doorSprite;
        public BoxCollider2D myCollider;

        private void Update()
        {
            if (!Input.GetButtonDown("Use")) return;
            
            if (!playerInRage || thisDoorType != DoorType.Key) return;
            
            if (playerInventory.numberOfKeys <= 0) return;
            
            playerInventory.numberOfKeys--;
            OpenDoor();
        }

        public void OpenDoor()
        {
            doorSprite.enabled = false;
            opened = true;
            myCollider.enabled = false;
        }

        public void CloseDoor()
        {
            doorSprite.enabled = true;
            opened = false;
            myCollider.enabled = true;
        }
    }
}
