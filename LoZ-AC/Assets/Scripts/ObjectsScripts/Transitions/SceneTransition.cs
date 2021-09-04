using Cameras;
using PlayerScripts;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace ObjectsScripts.Transitions
{
    public class SceneTransition : MonoBehaviour
    {
        [Header("New Scene Stats")]
        public string sceneToLoad;
        public VectorValue playerStorage;
        public Vector2 camNewMax;
        public Vector2 camNewMin;
        public VectorValue camMin;
        public VectorValue camMax;
        public Vector2 playerPortingPosition;

        [Header("Transition Stats")]
        public GameObject fadeInPanel;
        public GameObject fadeOutPanel;
        public float fadeDelay;

        private void OnEnable()
        {
            FadeIn();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || other.isTrigger) return;
            
            FadeIn();

            SceneManager.LoadScene(sceneToLoad);
            Player.instance.transform.position = playerPortingPosition;
            
            ResetCameraBound();
            FadeOut();
        }

        /// <summary>
        /// White fadeout.
        /// </summary>
        private void FadeOut()
        {
            if (fadeOutPanel == null) return;
            
            var panel = Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
            Destroy(panel, fadeDelay);
        }
        
        /// <summary>
        /// White fadein.
        /// </summary>
        private void FadeIn()
        {
            if (fadeInPanel == null) return;
            
            var panel = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity);
            Destroy(panel, fadeDelay);
        }

        private void ResetCameraBound()
        {
            camMax.initialValue = camNewMax;
            camMin.initialValue = camNewMin;
        }
    }
}
