using System;
using ObjectsScripts;
using PlayerScripts;
using ScriptableObjects;
using UnityEngine;

namespace GameStuff
{
    public class Sleep : InteractableObject
    {
        public Signal sleeping;
        private bool _isIdle;
        private bool _isWalk;
        private bool _isInteract;

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetButtonDown("Use") && playerInRage && (_isIdle || _isWalk) && !_isInteract)
            {
                sleeping.Raise();
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

        private void OnTriggerStay2D(Collider2D other)
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
