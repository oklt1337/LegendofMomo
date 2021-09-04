using System.Collections;
using PlayerScripts;
using ScriptableObjects;
using UnityEngine;

namespace Cameras
{
    public class CameraMovement : MonoBehaviour
    {
        [Header("Camera Stats")]
        public Transform target;
        public float smoothing;
        public Vector2 maxPosition;
        public Vector2 minPosition;

        [Header("Position Reset")] 
        public VectorValue camMin;
        public VectorValue camMax;
        
        private Animator _animator;
        private static readonly int Shaking = Animator.StringToHash("shaking");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            maxPosition = camMax.initialValue;
            minPosition = camMin.initialValue;
        }

        private void LateUpdate()
        {
            SmoothCameraMovement(target != null ? target : Player.instance.transform);
        }
    

        /// <summary>
        /// Follows the my target smoothly.
        /// </summary>
        private void SmoothCameraMovement(Transform myTarget)
        {
            var position = myTarget.position;
            var selfPosition = transform.position;
            var targetPosition = new Vector3(position.x, position.y, selfPosition.z);
            
            targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);
            selfPosition = Vector3.Lerp(selfPosition, targetPosition, smoothing);
            
            transform.position = selfPosition;
        }

        /// <summary>
        /// Shake coroutine.
        /// </summary>
        /// <returns></returns>
        private IEnumerator ShakeCo()
        {
            yield return null;
            _animator.SetBool(Shaking, false);
        }
        
        /// <summary>
        /// Starts shaking the screen.
        /// </summary>
        public void StartShake()
        {
            _animator.SetBool(Shaking, true);
            StartCoroutine(ShakeCo());
        }
    }
}
