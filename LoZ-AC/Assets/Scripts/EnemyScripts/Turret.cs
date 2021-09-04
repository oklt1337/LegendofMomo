using System;
using ObjectsScripts;
using UnityEngine;

namespace EnemyScripts
{
    public class Turret : Log
    {
        public GameObject bullet;
        public float delay;
        public bool canFire = true;
        
        private float _delay;

        private void Update()
        {
            _delay -= Time.deltaTime;
            
            if (!(_delay <= 0)) return;
            
            canFire = true;
            _delay = delay;
        }

        protected override void CheckDistance()
        {
            if (!(Vector3.Distance(target.position, transform.position) <= chaseRadius) ||
                !(Vector3.Distance(target.position, transform.position) > attackRadius)) return;
            
            if (currentState != EnemyState.Idle &&
                (currentState != EnemyState.Walk || currentState == EnemyState.Stagger)) return;
            
            if (!canFire) return;
            
            var position = transform.position;
            var tempVector = target.transform.position - position;
            var current = Instantiate(bullet, position, Quaternion.identity);
                
            current.GetComponent<Shooter>().Launch(tempVector);
            canFire = false;

            ChangeState(EnemyState.Attack);
        }
    }
}
