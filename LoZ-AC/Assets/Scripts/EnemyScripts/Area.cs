using UnityEngine;

namespace EnemyScripts
{
    public class Area : Log
    {
        public Collider2D boundary;

        protected override void CheckDistance()
        {
            if (Vector3.Distance(target.position, transform.position) <= chaseRadius 
                && Vector3.Distance(target.position, transform.position) > attackRadius && boundary.bounds.Contains(target.transform.position))
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
            else if (Vector3.Distance(target.position, transform.position) > chaseRadius || !boundary.bounds.Contains(target.transform.position))
            {
                var position = transform.position;
                var temp = Vector3.MoveTowards(position, target.position, moveSpeed * Time.fixedDeltaTime);
                
                atHome = MoveHome();
                ChangeAnimator(temp - position);

                if (!atHome) return;
                
                animator.SetBool(WakeUp, false);
            }
        }
    }
}
