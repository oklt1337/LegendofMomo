using System;
using EnemyScripts;
using ObjectsScripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerScripts
{
    public class KnockbackPlayer : MonoBehaviour
    {
        [Header("Kockback Stats")]
        public float force;
        public float knockBackTime;
        public float damage;
        
        public Player player;
        
        

        /// <summary>
        /// Makes Enemy Knock back.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("breakable") && gameObject.CompareTag("Player"))
            {
                other.GetComponent<Pot>().Smash();
            }

            if (!other.gameObject.CompareTag("enemy") && !other.gameObject.CompareTag("boss")) return;
            var hit = other.GetComponent<Rigidbody2D>();

            if (hit == null) return;

            if (other.gameObject.CompareTag("enemy"))
            {
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * force;
                hit.AddForce(difference, ForceMode2D.Impulse);
                    
                hit.GetComponent<Enemy>().currentState = EnemyState.Stagger;
                other.GetComponent<Enemy>().Knock(knockBackTime, player.Damage(damage));
            }

            if (other.gameObject.CompareTag("boss") && other.isTrigger && !other.CompareTag("bullet"))
            {
                other.GetComponent<Boss>().Knock(player.Damage(damage));
            }
        }
    }
}
