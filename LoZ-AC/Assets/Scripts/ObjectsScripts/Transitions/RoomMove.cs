using System;
using System.Collections;
using Cameras;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Canvas = GameStuff.Canvas;

namespace ObjectsScripts.Transitions
{
    public class RoomMove : MonoBehaviour
    {
        [Header("Vector Changes")]
        public Vector2 camMinChange;
        public Vector2 camMaxChange;
        public Vector3 playerChange;
        
        [Header("UI")]
        public bool needText;
        public string placeName;
        public GameObject text;
        public Text placeText;
    
        
        private CameraMovement _cam;
        private const float Delay = 1f;

        private void Awake()
        {
            StartCoroutine(GetDialogBoxCo());
        }

        private IEnumerator GetDialogBoxCo()
        {
            yield return new WaitForSeconds(.05f);
            text = Canvas.instance.transform.GetChild(0).gameObject;
            placeText = Canvas.instance.transform.GetChild(0).GetComponent<Text>();
        }
        
        // Start is called before the first frame update
        private void Start()
        {
            if (!(Camera.main is null)) _cam = Camera.main.GetComponent<CameraMovement>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || other.isTrigger) return;
            
            MoveRoom(other);
        }

        /// <summary>
        /// Moves Player to a new room and sets Cam max and min Position.
        /// </summary>
        /// <param name="other"></param>
        private void MoveRoom([NotNull] Collider2D other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            
            _cam.minPosition += camMinChange;
            _cam.maxPosition += camMaxChange;
            other.transform.position += playerChange;
            
            if (!needText) return;
            
            StartCoroutine(PlaceNameCo());
        }
    
        private IEnumerator PlaceNameCo()
        {
            text.SetActive(true);
            placeText.text = placeName;
            
            yield return new WaitForSeconds(Delay);
            
            text.SetActive(false);
        }
    
    }
}
