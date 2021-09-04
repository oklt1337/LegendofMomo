using System;
using System.Collections;
using ObjectsScripts.Doors;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using Canvas = GameStuff.Canvas;

namespace ObjectsScripts
{
    public class Switch : InteractableObject
    {
        [Header("Switch Stats")] 
        public bool isActive;
        public Bool switched;
        public Sprite activeSprite;
        public Door thisDoor;
        public GameObject dialogBox;
        public Text dialogText;
        public string dialog;

        private SpriteRenderer _mySprite;

        private void Awake()
        {
            StartCoroutine(GetDialogBoxCo());
        }
        
        private IEnumerator GetDialogBoxCo()
        {
            yield return new WaitForSeconds(.05f);
            dialogBox = Canvas.instance.transform.GetChild(1).gameObject;
            dialogText = dialogBox.transform.GetChild(0).GetComponent<Text>();
        }

        // Start is called before the first frame update
        private void Start()
        {
            _mySprite = GetComponent<SpriteRenderer>();
        }

        public void Update()
        {
            isActive = switched.runtimeValue;
            if (Input.GetKeyDown(KeyCode.E) || isActive)
            {
                ActivateSwitch();
            }

            if (!Input.GetKeyDown(KeyCode.E) || !isActive || !playerInRage) return;
            
            if (dialogBox.activeInHierarchy)
            {
                dialogBox.SetActive(false);
            }
            else
            {
                dialogBox.SetActive(true);
                dialogText.text = dialog;
            }
        }
            
        private void ActivateSwitch()
        {
            isActive = true;
            thisDoor.OpenDoor();
            _mySprite.sprite = activeSprite;
            switched.runtimeValue = isActive;
        }

        public override void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || other.isTrigger) return;
            
            context.Raise();
            playerInRage = false;
            dialogBox.SetActive(false);
        }
    }
}
