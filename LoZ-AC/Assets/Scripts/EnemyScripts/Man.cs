using System;
using UnityEngine;

namespace EnemyScripts
{
    public class Man : Enemy
    {
        [Header("Path Stats")]
        public float tolerance;
        public int currentPoint;
        public Transform nextGoal; 
        public Transform[] path;
        
        [Header("Components")]
        public CircleCollider2D myAttackTrigger;
        
        private static readonly int MovingID = Animator.StringToHash(Moving);
        private const string Moving = "moving";

        // Update is called once per frame
        private void FixedUpdate()
        {
            CheckDistance();
        }

        /// <summary>
        /// Makes enemy move between 2 points.
        /// </summary>
        private void ChangeGoal()
        {
            if (currentPoint == path.Length - 1)
            {
                currentPoint = 0;
                nextGoal = path[0];
            }
            else
            {
                currentPoint++;
                nextGoal = path[currentPoint];
            }
        }

        /// <summary>
        /// Distance check if in range move to target.
        /// </summary>
        protected override void CheckDistance()
        {
            SetMoveID(Moving);
            if (Vector3.Distance(target.position, transform.position) <= chaseRadius 
                && Vector3.Distance(target.position, transform.position) > attackRadius)
            {
                if (currentState != EnemyState.Idle &&
                    (currentState != EnemyState.Walk || currentState == EnemyState.Stagger)) return;
                
                var position = transform.position;
                var temp = Vector3.MoveTowards(position, target.position, moveSpeed * Time.fixedDeltaTime);
                
                ChangeAnimator(temp - position);
                
                myRigidbody.MovePosition(temp);
                animator.SetBool(MovingID, true);
            }
            else if (Vector3.Distance(target.position, transform.position) > chaseRadius)
            {
                if (Vector3.Distance(transform.position,path[currentPoint].position) > tolerance)
                {
                    var position = transform.position;
                    var temp = Vector3.MoveTowards(position, path[currentPoint].position, moveSpeed * Time.fixedDeltaTime);
                    
                    ChangeAnimator(temp - position);
                    
                    animator.SetBool(MovingID, true);
                    myRigidbody.MovePosition(temp);
                }
                else
                {
                    ChangeGoal();
                }
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
