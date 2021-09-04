using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Canvas = GameStuff.Canvas;

namespace ObjectsScripts.Doors
{
    public class ClosedDoor : Door
    {
        [Header("ClosedDoor Stats")]
        public GameObject dialogBox;
        public Text dialogText;
        public string dialog;

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
        
        // Update is called once per frame
        private void Update()
        {
            if (!Input.GetButtonDown("Use") || !playerInRage || thisDoorType != DoorType.Closed) return;
            
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
        
        public override void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || other.isTrigger) return;

            context.Raise();
            playerInRage = true;
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
