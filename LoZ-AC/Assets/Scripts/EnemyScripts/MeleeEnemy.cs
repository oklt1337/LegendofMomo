using System.Collections;
using UnityEngine;

namespace EnemyScripts
{
    public class MeleeEnemy : Enemy
    {
        private const float Delay = 1f;

        private const string IsMoving = "moving";
        private static readonly int GotHit = Animator.StringToHash("gotHit");
        private static readonly int Attacking = Animator.StringToHash("attacking");
        private static readonly int Moving = Animator.StringToHash(IsMoving);

        private void FixedUpdate()
        {
            SetMoveID(IsMoving);
            CheckDistance();
        }
        
        /// <summary>
        /// Distance check if in range move to target.
        /// </summary>
        protected override void CheckDistance()
        {
            if (Vector3.Distance(target.position, transform.position) <= chaseRadius 
                && Vector3.Distance(target.position, transform.position) > attackRadius && currentState != EnemyState.Attack)
            {
                if (currentState != EnemyState.Idle &&
                    (currentState != EnemyState.Walk || currentState == EnemyState.Stagger)) return;
                
                var position = transform.position;
                var temp = Vector3.MoveTowards(position, target.position, moveSpeed * Time.fixedDeltaTime);
                
                ChangeAnimator(temp - position);
                ChangeState(EnemyState.Walk);
                
                if (currentState != EnemyState.Walk) return;
                
                myRigidbody.MovePosition(temp);
                animator.SetBool(Moving, true);
            }
            else if (Vector3.Distance(target.position, transform.position) > chaseRadius)
            {
                animator.SetBool(Moving, false);
            }
            else if (Vector3.Distance(target.position, transform.position) <= chaseRadius 
                     && Vector3.Distance(target.position,transform.position) <= attackRadius)
            {
                if (currentState != EnemyState.Idle &&
                    (currentState != EnemyState.Walk || currentState == EnemyState.Stagger)) return;
                
                StartCoroutine(AttackCo());
            }
        }
        
        /// <summary>
        /// Knock coroutine.
        /// </summary>
        /// <param name="knockBackTime"></param>
        /// <returns></returns>
        protected override IEnumerator KnockCo(float knockBackTime)
        {
            if (myRigidbody == null) yield break;
            animator.SetBool(GotHit,true);
            
            yield return new WaitForSeconds(knockBackTime);
            
            myRigidbody.velocity = Vector2.zero;
            myRigidbody.GetComponent<Enemy>().currentState = EnemyState.Idle;
            myRigidbody.velocity = Vector2.zero;
            animator.SetBool(GotHit,false);
        }

        /// <summary>
        /// Attack coroutine.
        /// </summary>
        /// <returns></returns>
        private IEnumerator AttackCo()
        {
            currentState = EnemyState.Attack;
            animator.SetBool(Attacking, true);

            yield return new WaitForSeconds(Delay);

            currentState = EnemyState.Idle;
            animator.SetBool(Attacking,false);
        }
    }
}
