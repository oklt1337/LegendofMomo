using EnemyScripts;
using ObjectsScripts;
using UnityEngine;

namespace GameStuff.ResetRoom
{
    public class Rooms : MonoBehaviour
    {
        public Enemy[] enemies;
        public Pot[] pots;

        public virtual void OnTriggerEnter2D(Collider2D other)
        {
            ActivateObjects(other);
        }

        public virtual void OnTriggerExit2D(Collider2D other)
        {
            DeactivateObjects(other);
        }

        protected static void ChangeActiveState(Component component,bool setActive)
        {
            component.gameObject.SetActive(setActive);
        }

        private void ActivateObjects(Collider2D other)
        {
            if (!other.CompareTag("Player") || other.isTrigger) return;
            
            //Activate Objects  
            foreach (var t in enemies)
            {
                ChangeActiveState(t, true);
            }

            foreach (var t in pots)
            {
                ChangeActiveState(t, true);  
            }
        }

        private void DeactivateObjects(Collider2D other)
        {
            if (!other.CompareTag("Player") || other.isTrigger) return;
            
            //Deactivate Objects
            foreach (var t in enemies)
            {
                ChangeActiveState(t, false);
            }

            foreach (var t in pots)
            {
                ChangeActiveState(t, false);  
            }
        }
    }
}
