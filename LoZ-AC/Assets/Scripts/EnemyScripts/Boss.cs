using System.Collections;
using ObjectsScripts;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EnemyScripts
{
    public enum BossState
    {
        Idle,
        Attack,
        Stagger,
        Dead
    }

    public class Boss : MonoBehaviour
    {
        [Header("State Machine")]
        public BossState currentState;
        
        [Header("Enemy Stats")]
        public float shootRadius;
        public FloatValue maxHealth;
        public string enemyName;
        
        [Header("Enemy Components")]
        public Transform target;
        public Rigidbody2D myRigidbody;
        public GameObject deathEffect;
        public GameObject bulletRight;
        public GameObject bulletLeft;
        public Animator animator;
        
        public float fireDelay;
        public bool canFire = true;

        private Vector2 _tempVector;
        private float _fireDelay;
        private float _health;
        private const float Delay = 1f;
        private static readonly int GotHit = Animator.StringToHash("gotHit");
        private static readonly int Attacking = Animator.StringToHash("attack");
        
        private static readonly int MoveX = Animator.StringToHash("moveX");
        private static readonly int MoveY = Animator.StringToHash("moveY");

        private void Awake()
        {
            _health = maxHealth.initialValue;
        }
        
        private void Start()
        {
            myRigidbody = GetComponent<Rigidbody2D>();
            target = GameObject.FindWithTag("Player").transform;

            currentState = BossState.Idle;
        }

        private void Update()
        {
            _fireDelay -= Time.deltaTime;
            
            if (!(_fireDelay <= 0)) return;
            
            canFire = true;
            _fireDelay = fireDelay;
            _tempVector = target.transform.position - transform.position;
        }

        private void FixedUpdate()
        {
            CheckDistance();
        }

        /// <summary>
        /// Starts knock coroutine.
        /// </summary>
        /// <param name="damage">float taken damage</param>
        public void Knock(float damage)
        {
            if (gameObject.activeSelf)
            {
                StartCoroutine(KnockCo());
            }
            TakeDamage(damage);
        }
        
        /// <summary>
        /// Enemy takes damage and if hp zero, enemy dead.
        /// </summary>
        /// <param name="myDamage">float damagetaken</param>
        private void TakeDamage(float myDamage)
        {
            _health -= myDamage;
            if (!(_health <= 0)) return;
            currentState = BossState.Dead;
            
            DeathEffect();
            gameObject.SetActive(false);
            Death();
        }
        
        /// <summary>
        /// Instantiate death effect.
        /// </summary>
        private void DeathEffect()
        {
            if (deathEffect == null) return;
            
            var effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            
            Destroy(effect, Delay);
        }

        private void ChangeState(BossState newState)
        {
            // ReSharper disable once RedundantCheckBeforeAssignment
            if (currentState == newState) return;
            
            currentState = newState;
        }

        private void CheckDistance()
        {
            var position = transform.position;

            if (Vector3.Distance(target.position, position) <= shootRadius)
            {
                if (currentState != BossState.Idle &&
                    currentState == BossState.Stagger) return;

                ChangeAnimator(_tempVector);

                if (!canFire) return;

                ChangeState(BossState.Attack);

                if (currentState == BossState.Attack)
                {
                    canFire = false;
                    StartCoroutine(AttackCo());
                }
            }
            else if (!(Vector3.Distance(target.position, position) <= shootRadius))
            {
                ChangeState(BossState.Idle);
            }
        }
        
        /// <summary>
        /// Changes move animation.
        /// </summary>
        /// <param name="direction">Vector2 direction.</param>
        private void ChangeAnimator(Vector2 direction)
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
        /// Sets animator float.
        /// </summary>
        /// <param name="moveX">float</param>
        /// <param name="moveY">float</param>
        /// <param name="setVector">Vector2</param>
        private void SetAnimatorFloat(int moveX, int moveY, Vector2 setVector)
        {
            animator.SetFloat(moveX, setVector.x);
            animator.SetFloat(moveY, setVector.y);
        }

        /// <summary>
        /// Knock coroutine.
        /// </summary>
        /// <returns></returns>
        private IEnumerator KnockCo()
        {
            if (myRigidbody == null) yield break;
            animator.SetBool(GotHit,true);
            
            yield return new WaitForSeconds(_fireDelay);
            
            animator.SetBool(GotHit,false);
        }
        
        /// <summary>
        /// Attack coroutine.
        /// </summary>
        /// <returns></returns>
        private IEnumerator AttackCo()
        {
            if (animator == null) yield break;
            
            animator.SetBool(Attacking, true);
            
            yield return new WaitForSeconds(Delay);
            
            var position = transform.position;
            if (BulletDirection(_tempVector))
            {
                var current = Instantiate(bulletRight, position, Quaternion.identity);
                current.GetComponent<Shooter>().Launch(_tempVector);
            }
            else
            {
                var current = Instantiate(bulletLeft, position, Quaternion.identity);
                current.GetComponent<Shooter>().Launch(_tempVector);
            }
            animator.SetBool(Attacking,false);
        }
        
        /// <summary>
        /// Bullet direction bool.
        /// </summary>
        /// <param name="direction">Vactor2</param>
        /// <returns></returns>
        private static bool BulletDirection(Vector2 direction)
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x > 0)
                {
                    return true;
                }
                if (direction.x < 0)
                {
                    return false;
                }
            }else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
            {
                if (direction.y > 0)
                {
                    return true;
                }
                if (direction.y < 0)
                {
                    return false;
                }
            }
            return false;
        }
        
        /// <summary>
        /// Loads gameover scene and destroys player.
        /// </summary>
        private void Death()
        {
            if (currentState != BossState.Dead) return;
            
            SceneManager.LoadScene("WonGame");
            Destroy(gameObject);
        }
    }
}