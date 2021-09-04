using ObjectsScripts;
using PlayerScripts;
using UnityEngine;

namespace EnemyScripts
{
    public class KnockbackEnemy : MonoBehaviour
    {
        [Header("Kockback Stats")]
        public float force;
        public float knockBackTime;
        public float damage;

        /// <summary>
        /// Makes Player Knock back.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player") && !other.CompareTag("bullet")) return;
            
            var hit = other.GetComponent<Rigidbody2D>();

            if (hit == null) return;

            if (other.gameObject.CompareTag("Player") ||
                other.CompareTag("bullet"))
            {
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * force;
                hit.AddForce(difference, ForceMode2D.Impulse);
            }

            if (!other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("enemy") ||
                other.gameObject.CompareTag("boss") || other.CompareTag("bullet") || !other.isTrigger) return;
            if (other.GetComponent<Player>().currentState == PlayerState.Stagger) return;

            hit.GetComponent<Player>().currentState = PlayerState.Stagger;
            other.GetComponent<Player>().Knock(knockBackTime, damage);
        }
    }
}
