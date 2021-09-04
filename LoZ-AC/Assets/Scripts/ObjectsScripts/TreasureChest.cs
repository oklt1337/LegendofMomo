using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using Canvas = GameStuff.Canvas;

namespace ObjectsScripts
{
    public class TreasureChest : InteractableObject
    {
        [Header("TreasureChest Stats")]
        public Inventory playerInventory;
        public Item contents;
        public Bool chestOpened;
        public bool isOpen;
        
        [Header("Dialog")]
        public GameObject dialogBox;
        public Text dialogText;
        
        [Header("Signal")]
        public Signal receiveItem;
        
        private Animator _animator;
        private static readonly int Opened = Animator.StringToHash("opened");

        private void Awake()
        {
            dialogBox = Canvas.instance.transform.GetChild(1).gameObject;
            dialogText = dialogBox.transform.GetChild(0).GetComponent<Text>();
        }
        
        // Start is called before the first frame update
        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        private void Update()
        {
            isOpen = chestOpened.runtimeValue;

            if (Input.GetButtonDown("Use") && playerInRage)
            {
                if (isOpen)
                {
                    OpenedChest();
                }
                else
                {
                    OpenChest();
                }
            }
            
            if (isOpen) _animator.SetBool(Opened, true);
        }

        /// <summary>
        /// Tells Item Name and gives player the Item and make player animate and sets Chest state to Open.
        /// </summary>
        private void OpenChest()
        {
            dialogBox.SetActive(true);
            dialogText.text = contents.itemDescription;
            
            playerInventory.AddItem(contents);
            playerInventory.currentItem = contents;
            receiveItem.Raise();
            
            isOpen = true;
            context.Raise();
            _animator.SetBool(Opened,true);
            chestOpened.runtimeValue = isOpen;
        }

        /// <summary>
        /// Tells the Chest is already open, stops animating.
        /// </summary>
        private void OpenedChest()
        {
            dialogBox.SetActive(false);
            receiveItem.Raise();
        }

        public override void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || other.isTrigger || isOpen) return;
            
            context.Raise();
            playerInRage = true;
        }

        public override void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || other.isTrigger || isOpen) return;
            
            context.Raise();
            playerInRage = false;
        }
    }
}
