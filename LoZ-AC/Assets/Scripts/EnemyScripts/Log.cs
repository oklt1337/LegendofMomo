using System;
using UnityEngine;

namespace EnemyScripts
{
    public class Log : Enemy
    {
        public CircleCollider2D myAttackTrigger;
        
        protected static readonly int WakeUp = Animator.StringToHash(Moving);
        private const string Moving = "wakeUp";
    

        // Update is called once per frame
        private void FixedUpdate()
        {
            SetMoveID(Moving);
            CheckDistance();
        }
        
        /// <summary>
        /// Enemy moves home if target isn't anymore in rage.
        /// </summary>
        /// <returns>bool atHome</returns>
        protected bool MoveHome()
        {
            var position = transform.position;
            var temp = Vector3.MoveTowards(position, homePosition, moveSpeed * Time.fixedDeltaTime);
            
            ChangeState(EnemyState.Walk);
            ChangeAnimator(temp - position);
            
            myRigidbody.MovePosition(temp);
            
            if (homePosition != position) return position == homePosition;
            
            ChangeState(EnemyState.Idle);
            
            return position == homePosition;
        }
        
        /// <summary>
        /// Distance check if in range move to target.
        /// </summary>
        protected override void CheckDistance()
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
                animator.SetBool(WakeUp, true);
            }
            else if (Vector3.Distance(target.position, transform.position) > chaseRadius)
            {
                var position = transform.position;
                var temp = Vector3.MoveTowards(position, target.position, moveSpeed * Time.fixedDeltaTime);
                
                atHome = MoveHome();
                ChangeAnimator(temp - position);

                if (!atHome) return;
                
                animator.SetBool(WakeUp, false);
            }
        }
        
        /// <summary>
        /// Sets attack trigger radius.
        /// </summary>
        /// <exception cref="Exception"></exception>
        protected override void SetAttackTrigger()
        {
            if (myAttackTrigger.isTrigger)
            {
                myAttackTrigger.radius = attackRadius;
            }
            else
            {
                throw new Exception("Collider is no trigger.");
            }
        }
    }
}
