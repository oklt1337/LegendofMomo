using System.Linq;
using EnemyScripts;
using ObjectsScripts;
using ObjectsScripts.Doors;
using UnityEngine;

namespace GameStuff.ResetRoom
{
    public class MonsterRoom : LowerDungeonRooms
    {
        public Door[] doors;
        public int deadEnemys;
        
        public void EnemyCheck()
        {
            foreach (var t in enemies)
            {
                if (t.currentState == EnemyState.Dead && deadEnemys == enemies.Length)
                {
                    OpeningDoors();
                }
            }
        }

        private void ClosingDoors()
        {
            foreach (var t in doors)
            {
                t.CloseDoor();
            }
        }

        private void OpeningDoors()
        {
            foreach (var t in doors)
            {
                t.OpenDoor();
            }
        }
        
        public override void OnTriggerEnter2D(Collider2D other)
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

            ClosingDoors();
        }
        
        public override void OnTriggerExit2D(Collider2D other)
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

            deadEnemys = 0;
        }

        public void DeadEnemy()
        {
            deadEnemys++;
        }
    }
}
