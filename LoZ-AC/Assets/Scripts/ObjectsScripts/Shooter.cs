using System;
using UnityEngine;

namespace ObjectsScripts
{
    public class Shooter : MonoBehaviour
    {
        [Header("Movement")]
        public float speed;
        public Vector2 direction;
        
        [Header("Lifetime")]
        public float lifeTime;
        
        [Header("Components")]
        public Rigidbody2D myRigidbody2D;
        
        private float _lifeTime;
        
        // Start is called before the first frame update
        private void Start()
        {
            myRigidbody2D = GetComponent<Rigidbody2D>();
            _lifeTime = lifeTime;
        }

        // Update is called once per frame
        public void Update()
        {
            _lifeTime -= Time.deltaTime;
            
            if (!(_lifeTime <= 0)) return;
            
            Destroy(gameObject);
        }

        public void Launch(Vector2 chase)
        {
            myRigidbody2D.velocity = chase * speed;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            Destroy(gameObject);
        }
    }
}
