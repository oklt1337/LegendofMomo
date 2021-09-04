using System;
using PlayerScripts;
using ScriptableObjects;
using UnityEngine;


namespace ObjectsScripts
{
    public class Water : InteractableObject
    {
        public Signal fishing;
        private bool _isIdle;
        private bool _isWalk;
        private bool _isInteract;
        private void Update()
        {
            if (Input.GetButtonDown("Use") && playerInRage && (_isIdle || _isWalk) && !_isInteract)
            {
               fishing.Raise(); 
            }
        }
        
        public override void OnTriggerEnter2D(Collider2D other)
        {
            _isIdle = other.GetComponent<Player>().currentState == PlayerState.Idle;
            _isWalk = other.GetComponent<Player>().currentState == PlayerState.Walk;
            _isInteract = other.GetComponent<Player>().currentState == PlayerState.Interact;

            if (!other.CompareTag("Player") || other.isTrigger || _isInteract) return;
            
            context.Raise();
            playerInRage = true;
        }

        private void OnTriggerStay(Collider other)
        {
            _isInteract = other.GetComponent<Player>().currentState == PlayerState.Interact;
        }

        public override void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || other.isTrigger) return;
            
            context.Raise();
            playerInRage = false;
        }
    }
}
