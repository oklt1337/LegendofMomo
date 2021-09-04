using System;
using System.Collections;
using ScriptableObjects;
using UnityEngine;

namespace EnemyScripts
{
    public enum EnemyState
    {
        Idle,
        Walk,
        Attack,
        Stagger,
        Dead
    }
    [RequireComponent(typeof(Rigidbody2D ),typeof(Animator))]
    public class Enemy : MonoBehaviour
    {
        [Header("State Machine")]
        public EnemyState currentState;

        [Header("Enemy Stats")]
        public float chaseRadius;
        public float attackRadius;
        public FloatValue maxHealth;
        public string enemyName;
        public float baseAttack;
        public float moveSpeed;
        public bool atHome = true;
        
        [Header("Enemy Components")]
        public Vector3 homePosition;
        public Transform target;
        public Rigidbody2D myRigidbody;
        public GameObject deathEffect;

        [Header("Lootlist")]
        public LootList loot;
        
        [Header("Animator")]
        public Animator animator;

        protected float health;
        private static string _moving;
        private const float Delay = 1f;

        private static readonly int MoveX = Animator.StringToHash("moveX");
        private static readonly int MoveY = Animator.StringToHash("moveY");
        private static readonly int Walking = Animator.StringToHash(_moving);
        
        private void Awake()
        {
            SetAttackTrigger();
            health = maxHealth.initialValue;
        }

        private void OnEnable()
        {
            transform.position = homePosition;
            currentState = EnemyState.Idle;
            health = maxHealth.initialValue;
        }

        private void Start()
        {
            myRigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            target = GameObject.FindWithTag("Player").transform;
            
            var position = transform.position;
            
            currentState = EnemyState.Idle;
            homePosition = new Vector3(position.x, position.y,position.z);
        }
        
        /// <summary>
        /// Enemy takes damage and if hp zero, enemy dead.
        /// </summary>
        /// <param name="damage">float damagetaken</param>
        protected virtual void TakeDamage(float damage)
        {
            health -= damage;
            if (!(health <= 0)) return;
            
            DeathEffect();
            LootSpawner();
            
            currentState = EnemyState.Dead;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Instantiate death effect.
        /// </summary>
        protected void DeathEffect()
        {
            if (deathEffect == null) return;
            
            var effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            
            Destroy(effect, Delay);
        }

        protected virtual IEnumerator KnockCo(float knockBackTime)
        {
            if (myRigidbody == null) yield break;
            
            yield return new WaitForSeconds(knockBackTime);
            
            myRigidbody.velocity = Vector2.zero;
            myRigidbody.GetComponent<Enemy>().currentState = EnemyState.Idle;
            myRigidbody.velocity = Vector2.zero;
        }
    
        /// <summary>
        /// Sets move animation.
        /// </summary>
        private void SetAnimatorFloat(int moveX, int moveY, Vector2 setVector)
        {
            animator.SetFloat(moveX, setVector.x);
            animator.SetFloat(moveY, setVector.y);
        }
        
        /// <summary>
        /// Changes the playerstate.
        /// </summary>
        /// <param name="newState">Enum</param>
        protected void ChangeState(EnemyState newState)
        {
            // ReSharper disable once RedundantCheckBeforeAssignment
            if (currentState == newState) return;
            
            currentState = newState;
        }
        
        /// <summary>
        /// Distance check if in range move to target.
        /// </summary>
        protected virtual void CheckDistance()
        {
            if (Vector3.Distance(target.position, transform.position) <= chaseRadius 
                && Vector3.Distance(target.position, transform.position) > attackRadius)
            {
                if (currentState != EnemyState.Idle &&
                    (currentState != EnemyState.Walk || currentState == EnemyState.Stagger)) return;
                
                var position = transform.position;
                var temp = Vector3.MoveTowards(position, target.position, moveSpeed * Time.fixedDeltaTime);
                
                ChangeAnimator(temp - position);
                ChangeState(EnemyState.Walk);
                
                myRigidbody.MovePosition(temp);
                animator.SetBool(Walking, true);
            }
            else if (Vector3.Distance(target.position, transform.position) > chaseRadius)
            {
                animator.SetBool(Walking, false);
            }
        }

        protected virtual void SetAttackTrigger()
        {
        }
        
        /// <summary>
        /// Changes move animation.
        /// </summary>
        /// <param name="direction">Vector2 direction.</param>
        protected void ChangeAnimator(Vector2 direction)
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x > 0)
                {
                    SetAnimatorFloat(MoveX, MoveY, Vector2.right);
                }else if (direction.x < 0)
                {
                    SetAnimatorFloat(MoveX, MoveY, Vector2.left);
                }
            }else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
            {
                if (direction.y > 0)
                {
                    SetAnimatorFloat(MoveX, MoveY, Vector2.up);
                }else if (direction.y < 0)
                {
                    SetAnimatorFloat(MoveX, MoveY, Vector2.down);
                }
            }
        }
        
        /// <summary>
        /// Sets moving bool.
        /// </summary>
        /// <param name="moving">bool</param>
        protected static void SetMoveID(string moving)
        {
            _moving = moving;
        }
        
        /// <summary>
        /// Starts knock coroutine.
        /// </summary>
        /// <param name="knockBackTime">float</param>
        /// <param name="damage">float taken damage</param>
        public void Knock(float knockBackTime, float damage)
        {
            if (gameObject.activeSelf)
            {
                StartCoroutine(KnockCo(knockBackTime));
            }
            TakeDamage(damage);
        }
        
        /// <summary>
        /// Spawns random loot.
        /// </summary>
        private void LootSpawner()
        {
            if (loot == null) return;
            
            var current = loot.LootDrop();
            
            if (current == null) return;
            
            Instantiate(current.gameObject, transform.position, Quaternion.identity);
        }
    }
}