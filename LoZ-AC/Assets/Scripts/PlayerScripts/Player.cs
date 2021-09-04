using System.Collections;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PlayerScripts
{
    public enum PlayerState
    {
        Walk,
        Attack,
        Interact,
        Stagger,
        Idle,
        Dead
    }

    public sealed class Player : MonoBehaviour
    {
        [Header("State Machine")]
        public PlayerState currentState;
        
        [Header("Player Instance")]
        public static Player instance;
        
        [Header("Animator Components")]
        public SpriteRenderer receivedItemSprite;

        [Header("Audio")] 
        public AudioSource swordSound;
        public AudioSource damageSound;
        public AudioSource sleepSound;

        [Header("Movement")]
        public float speed;
        public float runSpeed;
        
        [Header("FloatValues")]
        public FloatValue currentHealth;
        public FloatValue heartContainers;
        
        [Header("Signals")]
        public Signal playerHealthSignal;
        public Signal playerHit;
        public Signal context;
        public Signal dead;
        public Signal inventoryDisplay;

        [Header("Player Stats")] 
        public PlayerStats playerStats;
        
        [Header("DropItems")]
        public DropItems loot;
        public DropItems fruit;
        
        [Header("Dialog")]
        public GameObject dialogBox;
        public Text dialogText;
        public string noWeaponDialog;
        public string noRodDialog;
        public string fullLive;
        public string cantEat;
        public string already;
        public string gift;
        
        [Header("Player Inventory & Items")]
        public Inventory playerInventory;
        public Item runningShoes;
        public Item woodSword;
        public Item ironSword;
        public Item diamondSword;
        public Item rod;
        public Item[] itemToReset = new Item[12];
        public Item[] consumable = new Item[4];

        [Header("FadeIn & FadeOut Panel")] 
        public GameObject fadeInPanel;
        public GameObject fadeOutPanel;
        public float fadeDelay;

        [SerializeField]private VectorValue startingPosition;

        private float _speed;
        private const float Delay = .3f;
        private const float FishDelay = 5f;

        private Animator _animator;
        private Rigidbody2D _myRigidbody;
        private Vector3 _change;

        private static readonly int MoveX = Animator.StringToHash("moveX");
        private static readonly int MoveY = Animator.StringToHash("moveY");
        private static readonly int Moving = Animator.StringToHash("moving");
        
        private static readonly int AttackingWood = Animator.StringToHash("attackingWood");
        private static readonly int AttackingIron = Animator.StringToHash("attackingIron");
        private static readonly int AttackingDiamond = Animator.StringToHash("attackingDiamond");
        
        private static readonly int ReceiveItem = Animator.StringToHash("receiveItem");
        private static readonly int Fishing = Animator.StringToHash("fishing");
        
        private void Awake()
        {
            DestroyGameObject();
            
            _myRigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            
            DontDestroyOnLoad(gameObject);
            transform.position = startingPosition.initialValue;
            currentState = PlayerState.Idle;
        }

        private void Start()
        {
            FixAttackHitBox();
        }

        private void Update()
        {
            InputCheck();
            Once();
            Run();
        }
        
        /// <summary>    
        /// If gameobject already exist, Destroy it.
        /// </summary>
        private void DestroyGameObject()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            PlayerStatsReset();
            instance = this;
        }
        
        /// <summary>
        /// Resets player stats.
        /// </summary>
        private void PlayerStatsReset()    
        {
            playerInventory.coins = playerStats.playerInventory;
            playerInventory.numberOfKeys = playerStats.playerInventory;
            playerInventory.items.Clear();
            playerInventory.currentItem = null;
            
            foreach (var t in itemToReset)
            {
                t.amount = 0;
            }
            
            currentHealth.initialValue = playerStats.playerHealth;
            currentHealth.runtimeValue = currentHealth.initialValue;
            heartContainers.initialValue = playerStats.playerHeartContainers;

            _speed = playerStats.speed;
        }

        /// <summary>
        /// Items fix.
        /// </summary>
        private void Once()
        {
            foreach (var t in itemToReset)
            {
                if (!t.onlyOnce) continue;
                
                if (t.amount > 1)
                {
                    t.amount = 1;
                }
            }
        }
        
        /// <summary>
        /// Checks for input.
        /// </summary>
        private void InputCheck()
        {
            if (Input.GetKeyDown(KeyCode.Tab) && currentState != PlayerState.Attack &&
                     currentState != PlayerState.Stagger)
            {
                OpenInventory();
            }

            if (Input.GetKeyDown(KeyCode.Q) && currentState != PlayerState.Attack &&
                currentState != PlayerState.Stagger)
            {
                Eat();
            }

            if (currentState == PlayerState.Interact) return;
            
            _change = Vector3.zero;
            _change.x = Input.GetAxisRaw("Horizontal");
            _change.y = Input.GetAxisRaw("Vertical");

            if (Input.GetButtonDown("attack") && currentState != PlayerState.Attack &&
                currentState != PlayerState.Stagger &&
                (playerInventory.items.Contains(woodSword) || playerInventory.items.Contains(ironSword) ||
                 playerInventory.items.Contains(diamondSword)))
            {
                swordSound.Play();
                AttackAnimation();
            }
            else if (Input.GetButtonDown("attack") && currentState != PlayerState.Attack &&
                     currentState != PlayerState.Stagger &&
                     !(playerInventory.items.Contains(woodSword) || playerInventory.items.Contains(ironSword) ||
                       playerInventory.items.Contains(diamondSword)))
            {
                StartCoroutine(DialogCo(noWeaponDialog));
            }
            else if (currentState == PlayerState.Walk || currentState == PlayerState.Idle)
            {
                UpdateAnimationAndMove();
            }
        }
        
        /// <summary>
        /// CharacterMovement.
        /// </summary>
        private void MoveCharacter()
        {
            _change.Normalize();
            _myRigidbody.MovePosition(transform.position + _change * (speed * Time.fixedDeltaTime));
        }
        
        /// <summary>
        /// Makes player run.
        /// </summary>
        private void Run()
        {
            if (playerInventory.items.Contains(runningShoes))
            {
                speed = Input.GetButton("Run") ? runSpeed : _speed;
            }
        }

        private IEnumerator DialogCo(string dialog)
        {
            dialogBox.SetActive(true);
            dialogText.text = dialog;
            currentState = PlayerState.Interact;
            
            yield return new WaitForSeconds(Delay * 5f);
            dialogBox.SetActive(false);
            currentState = PlayerState.Idle;
        }
        
        /// <summary>
        /// Starts attack animation.
        /// </summary>
        private void AttackAnimation()
        {
            if (playerInventory.items.Contains(woodSword)&& !playerInventory.items.Contains(ironSword) && !playerInventory.items.Contains(diamondSword))
            {
                StartCoroutine(AttackWoodCo());
            }
            else if(playerInventory.items.Contains(ironSword)&& !playerInventory.items.Contains(diamondSword))
            {
                StartCoroutine(AttackIronCo());
            }
            else if (playerInventory.items.Contains(diamondSword))
            {
                StartCoroutine(AttackDiamondCo());
            }
        }

        /// <summary>
        /// Character movement + animation.
        /// </summary>
        private void UpdateAnimationAndMove()
        {
            if (_change != Vector3.zero)
            {
                MoveCharacter();
                //fix diagonal hitbox.
                _change.x = Mathf.Round(_change.x);
                _change.y = Mathf.Round(_change.y);
                
                _animator.SetFloat(MoveX, _change.x);
                _animator.SetFloat(MoveY, _change.y);
                _animator.SetBool(Moving, true);
            }
            else
            {
                _animator.SetBool(Moving,false);
            }
        }
        
        /// <summary>
        /// Fixes the hitbox on game start.
        /// </summary>
        private void FixAttackHitBox()
        {
            _animator.SetFloat(MoveX,0);
            _animator.SetFloat(MoveY, -1);
        }
        
        /// <summary>
        /// Sets bool for attack animation iron sword.
        /// </summary>
        private IEnumerator AttackIronCo()
        {
            _animator.SetBool(AttackingIron, true);
            currentState = PlayerState.Attack;
            
            yield return null;
            
            _animator.SetBool(AttackingIron, false);
            
            yield return new WaitForSeconds(Delay);
            
            if (currentState == PlayerState.Interact) yield break;
            
            currentState = PlayerState.Walk;
        }
        
        /// <summary>
        /// Sets bool for attack animation wood sword.
        /// </summary>
        private IEnumerator AttackWoodCo()
        {
            _animator.SetBool(AttackingWood, true);
            currentState = PlayerState.Attack;
            
            yield return null;
            
            _animator.SetBool(AttackingWood, false);
            
            yield return new WaitForSeconds(Delay);
            
            if (currentState == PlayerState.Interact) yield break;
            
            currentState = PlayerState.Walk;
        }
        
        
        /// <summary>
        /// Sets bool for attack animation diamond sword.
        /// </summary>
        private IEnumerator AttackDiamondCo()
        {
            _animator.SetBool(AttackingDiamond, true);
            currentState = PlayerState.Attack;
            
            yield return null;
            
            _animator.SetBool(AttackingDiamond, false);
            
            yield return new WaitForSeconds(Delay);
            
            if (currentState == PlayerState.Interact) yield break;
            
            currentState = PlayerState.Walk;
        }
        
        /// <summary>
        /// Animation for receiving an item.
        /// </summary>
        public void ReceivedItem()
        {
            if (playerInventory.currentItem == null) return;
            if (currentState != PlayerState.Interact)
            {
                _animator.SetBool(ReceiveItem, true);
                currentState = PlayerState.Interact;
                receivedItemSprite.sprite = playerInventory.currentItem.itemSprite;
            }
            else
            {
                _animator.SetBool(ReceiveItem, false);
                currentState = PlayerState.Idle;
                receivedItemSprite.sprite = null;
                playerInventory.currentItem = null;
            }
        }
        
        /// <summary>
        /// Loads gameover scene and destroys player.
        /// </summary>
        private void Death()
        {
            if (currentState != PlayerState.Dead) return;
            
            SceneManager.LoadScene("GameOver");
            PlayerStatsReset();
            dead.Raise();
            Destroy(gameObject);
        }
        
        private IEnumerator KnockCo(float knockBackTime)
        {
            playerHit.Raise();
            if (_myRigidbody == null) yield break;
            
            yield return new WaitForSeconds(knockBackTime);
            
            var velocity = Vector2.zero;
            
            _myRigidbody.velocity = velocity;
            currentState = PlayerState.Idle;
        }

        /// <summary>
        /// Sets health minus damage and starts knock coroutine.
        /// </summary>
        /// <param name="knockBackTime"></param>
        /// <param name="damage">float, taken damage</param>
        public void Knock(float knockBackTime, float damage)
        {
            TakeDamage(damage);
            
            if (currentHealth.runtimeValue > 0)
            {
                StartCoroutine(KnockCo(knockBackTime));
            }
            else
            {
                currentState = PlayerState.Dead;
                gameObject.SetActive(false);
                Death();
            }
        }

        /// <summary>
        /// Damage calculation.
        /// </summary>
        /// <param name="damage">float, taken damage</param>
        private void TakeDamage(float damage)
        {
            damageSound.Play();
            currentHealth.runtimeValue -= damage;
            playerHealthSignal.Raise(); 
        }

        /// <summary>
        /// Damage depend on weapon.
        /// </summary>
        /// <param name="damage">float default damage</param>
        /// <returns>damage</returns>
        public float Damage(float damage)
        {
            if (playerInventory.items.Contains(woodSword) && !playerInventory.items.Contains(ironSword) && !playerInventory.items.Contains(diamondSword))
            {
                return damage;
            }
            
            if(playerInventory.items.Contains(ironSword) && !playerInventory.items.Contains(diamondSword))
            {
                return damage * 2;
            }
            
            if (playerInventory.items.Contains(diamondSword))
            {
                return damage * 4;
            }
            return damage;
        }

        /// <summary>
        /// Fishing coroutine.
        /// </summary>
        /// <returns></returns>
        private IEnumerator FishCo()
        {
            _animator.SetBool(Fishing, true);
            context.Raise();
            currentState = PlayerState.Interact;
            
            yield return new WaitForSeconds(FishDelay);
            
            _animator.SetBool(Fishing, false);
            
            yield return null;
            
            LootSpawner();

            if (currentState == PlayerState.Interact) yield break;
            currentState = PlayerState.Idle;
        }
        
        /// <summary>
        /// Fruit coroutine.
        /// </summary>
        /// <returns></returns>
        private IEnumerator FruitCo()
        {
            context.Raise();
            currentState = PlayerState.Interact;
            
            FruitSpawner();
            
            currentState = PlayerState.Idle;
            if (currentState == PlayerState.Interact) yield break;
            currentState = PlayerState.Idle;
        }
        
        /// <summary>
        /// Sleeping coroutine.
        /// </summary>
        /// <returns></returns>
        private IEnumerator SleepCo()
        {
            context.Raise();
            currentState = PlayerState.Interact;
            
            FadeIn();
            
            yield return new WaitForSeconds(1f);
            
            sleepSound.Play();

            yield return null;
            
            currentHealth.runtimeValue = currentHealth.initialValue;
            FadeOut();
            
            if (currentState == PlayerState.Interact) yield break;
            currentState = PlayerState.Idle;
        }
        
        /// <summary>
        /// Market coroutine.
        /// </summary>
        /// <returns></returns>
        private IEnumerator MarketCo()
        {
            currentState = PlayerState.Interact;

            GiftedSword();

            currentState = PlayerState.Idle;
            if (currentState == PlayerState.Interact) yield break;
            currentState = PlayerState.Idle;
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
        /// Add random item to inventory.
        /// </summary>
        private void GiftedSword()
        {
            if (loot != null)
            {
                var current = diamondSword;

                if (current == null) return;
                if (playerInventory.items.Contains(current))
                {
                    StartCoroutine(DialogCo(already));
                }
                else
                {
                    playerInventory.AddItem(current);
                    playerInventory.currentItem = current;
                    
                    StartCoroutine(DialogCo(gift));
                    
                    if (playerInventory.currentItem == null) return;
                    if (currentState == PlayerState.Interact)
                    {
                        StartCoroutine(ReceiveCo(false));
                    }
                }
                
            }
            else
            {
                currentState = PlayerState.Idle;
            }
        }
        
        /// <summary>
        /// Add random item to inventory.
        /// </summary>
        private void FruitSpawner()
        {
            if (loot != null)
            {
                var current = fruit.LootDrop();

                if (current == null) return;

                playerInventory.AddItem(current);
                playerInventory.currentItem = current;

                if (playerInventory.currentItem == null) return;
                if (currentState == PlayerState.Interact)
                {
                    StartCoroutine(ReceiveCo(true));
                }
            }
            else
            {
                currentState = PlayerState.Idle;
            }
        }
        
        /// <summary>
        /// Add random item to inventory.
        /// </summary>
        private void LootSpawner()
        {
            if (loot == null) return;
            
            var current = loot.LootDrop();
            
            if (current == null) return;
            
            playerInventory.AddItem(current);
            playerInventory.currentItem = current;
            
            if (playerInventory.currentItem == null) return;
            if (currentState == PlayerState.Interact)
            {
                StartCoroutine(ReceiveCo(true));
            }
        }

        private IEnumerator ReceiveCo(bool needContext)
        {
            _animator.SetBool(ReceiveItem, true);
            currentState = PlayerState.Interact;
            receivedItemSprite.sprite = playerInventory.currentItem.itemSprite;
            
            yield return new WaitForSeconds(Delay * 3f);
            
            _animator.SetBool(ReceiveItem, false);
            
            if (needContext)
            {
                context.Raise(); 
            }
            
            
            receivedItemSprite.sprite = null;
            playerInventory.currentItem = null;
            
            currentState = PlayerState.Idle;
        }
        
        /// <summary>
        /// Starts fishing.
        /// </summary>
        public void Fish()
        {
            StartCoroutine(playerInventory.items.Contains(rod) ? FishCo() : DialogCo(noRodDialog));
        }
        
        /// <summary>
        /// Starts fruiting.
        /// </summary>
        public void Fruit()
        {
            StartCoroutine(FruitCo());
        }

        /// <summary>
        /// Get a gift.
        /// </summary>
        public void Market()
        {
            StartCoroutine(MarketCo());
        }

        /// <summary>
        /// Start sleeping.
        /// </summary>
        public void Sleep()
        {
            StartCoroutine(currentHealth.runtimeValue < currentHealth.initialValue ? SleepCo() : DialogCo(fullLive));
        }

        private void Eat()
        {
            if (currentHealth.runtimeValue < currentHealth.initialValue)
            {
                foreach (var t in consumable)
                {
                    if (!playerInventory.items.Contains(t)) continue;
                    
                    t.amount--;
                    
                    if (t.amount != 0) continue;
                    
                    playerInventory.items.Remove(t);
                    currentHealth.runtimeValue++;
                    return;
                }

                StartCoroutine(DialogCo(cantEat));
            }
            else
            {
                StartCoroutine(DialogCo(fullLive));
            }
        }

        private void OpenInventory()
        {
            currentState = currentState != PlayerState.Interact ? PlayerState.Interact : PlayerState.Idle;
            inventoryDisplay.Raise();
        }
    }
}