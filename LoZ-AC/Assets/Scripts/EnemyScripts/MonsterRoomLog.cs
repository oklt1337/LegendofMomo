using ScriptableObjects;
using UnityEngine;

namespace EnemyScripts
{
    public class MonsterRoomLog : Log
    {
        [Header("Death Signal")] 
        public Signal roomSignal;
        
        /// <summary>
        /// enemy takes damage.
        /// </summary>
        /// <param name="damage"></param>
        protected override void TakeDamage(float damage)
        {
            health -= damage;
            if (!(health <= 0)) return;
            
            DeathEffect();
            currentState = EnemyState.Dead;
            roomSignal.Raise();
            gameObject.SetActive(false);
        }
    }
}
