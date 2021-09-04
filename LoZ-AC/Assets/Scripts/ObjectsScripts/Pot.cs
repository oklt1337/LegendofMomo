using System.Collections;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace ObjectsScripts
{
    public class Pot : MonoBehaviour
    {
        public LootList loot;

        private const float Delay = .3f;
        private Animator _animator;
        private new static readonly int Destroy = Animator.StringToHash("smash");

        // Start is called before the first frame update
        private void Start()
        {
            _animator = GetComponent<Animator>();
        }
        

        /// <summary>
        /// Makes Pot break.
        /// </summary>
        public void Smash()
        {
            _animator.SetBool(Destroy, true);
            StartCoroutine(BreakCo());
        }

        /// <summary>
        /// Sets Pot to inactive.
        /// </summary>
        private IEnumerator BreakCo()
        {
            yield return new WaitForSeconds(Delay);
            LootSpawner();
            gameObject.SetActive(false);
        }

        private void LootSpawner()
        {
            if (loot == null) return;
            
            var current = loot.LootDrop();
            
            if (current == null) return;
            
            Instantiate(current.gameObject, transform.position, Quaternion.identity);
        }
    }
}
