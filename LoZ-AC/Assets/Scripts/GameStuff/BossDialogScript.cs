using System.Collections;
using ObjectsScripts;
using PlayerScripts;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GameStuff
{
    public class BossDialogScript : MonoBehaviour
    {
        public Signal bossDialog;
        
        [Header("Dialog")]
        public GameObject dialogBox;
        public Text dialogText;

        public string bDialog;
        public string bDialog1;
        
        private bool _isInteract;

        private void Awake()
        {
            dialogBox = Canvas.instance.transform.GetChild(1).gameObject;
            dialogText = dialogBox.transform.GetChild(0).GetComponent<Text>();
        }
        
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || other.isTrigger || other.GetComponent<Player>().currentState == PlayerState.Interact) return;
            other.GetComponent<Player>().currentState = PlayerState.Interact;
            BossDialog();
            other.GetComponent<Player>().currentState = PlayerState.Idle;
            Invoke("BossScene", 3f);
        }

        private void BossScene()
        {
            SceneManager.LoadScene("Palace");
        }
        
        private void BossDialog()
        {
            StartCoroutine(DialogCo(bDialog));
            StartCoroutine(DialogCo(bDialog1));
        }
        
        private IEnumerator DialogCo(string dialog)
        {
            dialogBox.SetActive(true);
            dialogText.text = dialog;

            yield return new WaitForSeconds(5f);
            dialogBox.SetActive(false);
            
        }
    }
}
