using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : BaseHuman
{
    private enum State
    {
        Idle,
        Move,
        Attack,
        Die
    }

    [SerializeField] private CameraRaycast cameraRaycast;
    [SerializeField] private GameObject playerWeapon1;
    [SerializeField] private GameObject playerWeapon2;
    [SerializeField] private GameObject bonusWeapon;

    [SerializeField] private Inventory inventory;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private EquipmentPanel ep;

    [SerializeField] private float attackDistance;
    [SerializeField] private float escapeTime;

    [Header("Animator")]
    private Animator animator;
    [SerializeField] private string isWalkingBoolName;
    [SerializeField] private string isRunningBoolName;
    [SerializeField] private string enemyNearbyTriggerName;
    [SerializeField] private string attackTriggerName;
    [SerializeField] private string isAttackingBoolName;
    [SerializeField] protected string isDeadTriggerName;

    [Header("NavMeshAgent")]
    [SerializeField] private LayerMask accesableArea;
    [SerializeField] private LayerMask unaccesableArea;
    [SerializeField] private float runningSpeed;
    [SerializeField] private float walkingSpeed;
    [SerializeField] private float minDistance;
    private NavMeshAgent navMeshAgent;
    private RaycastHit destinationInfo;

    [Header("Double click checker")]
    private const float timeBetweenClicks = 0.2f;
    private float firstClickTime = 0f;
    private bool isCoroutineDenied;
    private int clickCounter;

    private bool isAlive;
    private bool isFighting;
    private bool isCollecting;
    private bool isAgressive;
    private bool isTired;
    private PlayerStateMachine stateMachine;
    private State currentState;
    private EnemyFinder enemyFinder;

    public string EnemyNearbyTriggerName => enemyNearbyTriggerName;
    public Transform CachedTransform { get; private set; }
    public bool IsEscaping { get; private set; }
    public bool IsAlive => isAlive;


    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        enemyFinder = GetComponent<EnemyFinder>();
    }

    private void OnEnable()
    {
        cameraRaycast.OnPlayerMove += CameraRaycast_OnPlayerMove;
        cameraRaycast.OnPlayerStop += CameraRaycast_OnPlayerStop;
        cameraRaycast.OnPlayerAttack += CameraRaycast_OnPlayerAttack;
        cameraRaycast.OnPlayerInteract += CameraRaycast_OnPlayerInteract;
    }

    private void OnDisable()
    {
        cameraRaycast.OnPlayerMove -= CameraRaycast_OnPlayerMove;
        cameraRaycast.OnPlayerStop -= CameraRaycast_OnPlayerStop;
        cameraRaycast.OnPlayerAttack -= CameraRaycast_OnPlayerAttack;
        cameraRaycast.OnPlayerInteract -= CameraRaycast_OnPlayerInteract;
    }

    protected override void Start()
    {
        base.Start();

        CachedTransform = transform;
        cameraRaycast = FindObjectOfType<CameraRaycast>();
        stateMachine = new PlayerStateMachine();
        destinationInfo.point = transform.position;
        navMeshAgent.autoRepath = true;
        isAlive = true;


        SetState(State.Idle);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Item) && isCollecting)
        {
            var item = other.GetComponent<ItemCollector>().GetItem();
            isCollecting = false;
            inventory.IsAbleToAddItem(item);
            SetState(State.Idle);
            Destroy(other.gameObject);
        }
    }
    
    protected override void Update()
    {
        base.Update();

        if (isAlive)
        {
            UpdateCurrentCondition();
            CheckState();
            UpdateCurrentState();

            if (Input.GetMouseButtonUp(0))
            {
                clickCounter++;
            }

            if (clickCounter == 1 && !isCoroutineDenied)
            {
                CheckForSecondClick();
            }

        }
    }

    public Vector3 GetDestinationPoint()
    {
        return destinationInfo.point;
    }

    public void Escape()
    {
        IsEscaping = true;
        isFighting = false; ;
        StartCoroutine(OnEscape());
    }

    public void SetPlayerAgressive(bool isPlayerAgressive)
    {
        isAgressive = isPlayerAgressive;

        if (isAgressive)
        {
            animator.SetTrigger(EnemyNearbyTriggerName);
        }

        animator.SetBool(isAttackingBoolName, isPlayerAgressive);
        CheckForEquippedWeapon(isPlayerAgressive);
    }

    public void HealPlayer(float healValue)
    {
        if (CurrentHealth >= maxHealth)
        {
            return;
        }
        CurrentHealth += healValue;
        if (CurrentHealth >= maxHealth)
        {
            CurrentHealth = maxHealth;
        }
    }

    private void CheckState()
    {
        if (enemyFinder.ClosestBandit != null)
        {
            if (Vector3.Distance(CachedTransform.position, enemyFinder.ClosestBandit.transform.position) <= attackDistance && !IsEscaping)
            {
                Debug.Log("Attack");
                SetState(State.Attack);
            }
        }

        if (CurrentHealth <= 0 && isAlive)
        {
            SetState(State.Die);
        }
    }

    private void UpdateCurrentState()
    {
        if (currentState == State.Die)
        {
            Die();
            isAlive = false;
        }
        else if (currentState == State.Move && !isTired)
        {
            Move();
        }
        else if (currentState == State.Attack && !isTired)
        {
            Attack();
        }
        else if(currentState == State.Idle)
        {
            Idle();
        }
    }

    private void SetState(State newState)
    {
        switch (newState)
        {
            case State.Idle:
                {
                    animator.SetBool(isWalkingBoolName, false);
                    animator.SetBool(isRunningBoolName, false);

                    navMeshAgent.speed = walkingSpeed;
                    break;
                }
            case State.Move:
                {
                    animator.SetBool(isWalkingBoolName, true);
                    if (navMeshAgent.enabled == false)
                    {
                        navMeshAgent.enabled = true;
                    }
                    navMeshAgent.SetDestination(GetDestinationPoint());
                    break;
                }
            case State.Attack:
                {
                    navMeshAgent.enabled = false;
                    animator.SetBool(isWalkingBoolName, false);
                    animator.SetBool(isRunningBoolName, false);

                    break;
                }
            case State.Die:
                {
                    SetPlayerAgressive(false);
                    animator.SetBool(isAttackingBoolName, false);
                    navMeshAgent.enabled = false;
                    GetComponent<Collider>().enabled = false;

                    break;
                }
        }

        currentState = newState;
    }

    private void Idle()
    {
        if (CurrentStamina >= maxStamina)
        {
            return;
        }
        if (CurrentStamina > maxStamina * 0.25)
        {
            isTired = false;
        }
        CurrentStamina += CalculateStaminaExhaustion();
    }

    private void Move()
    {
        if (enemyFinder.ClosestBandit != null)
        {
            if (Vector3.Distance(CachedTransform.position, enemyFinder.ClosestBandit.CachedTransform.position) <= attackDistance && enemyFinder.ClosestBandit.CurrentHealth > 0 && !isFighting)
            {
                isFighting = true;
                destinationInfo.point = transform.position;
                navMeshAgent.SetDestination(destinationInfo.point);
            }
        }

        if (Vector3.Distance(CachedTransform.position, GetDestinationPoint()) <= minDistance)
        {
            SetState(State.Idle);
        }

        if (navMeshAgent.speed == runningSpeed)
        {
            if (CurrentStamina <= 0)
            {
                isTired = true;
                navMeshAgent.SetDestination(CachedTransform.position);
                SetState(State.Idle);
            }
            CurrentStamina -= CalculateStaminaExhaustion();
        }
        else if (navMeshAgent.speed == walkingSpeed)
        {
            if (CurrentStamina >= maxStamina)
            {
                return;
            }
            CurrentStamina += CalculateStaminaExhaustion();
        }
    }

    private void Attack()
    {
        if (enemyFinder.ClosestBandit != null)
        {
            if (Vector3.Distance(CachedTransform.position, enemyFinder.ClosestBandit.CachedTransform.position) <= attackDistance
            && enemyFinder.ClosestBandit.CurrentHealth > 0)
            {
                CachedTransform.LookAt(enemyFinder.ClosestBandit.CachedTransform.position);
                animator.SetTrigger(attackTriggerName);
            }
        }

        if (enemyFinder.ClosestBandit == null)
        {
            SetState(State.Move);
        }

        if (Vector3.Distance(CachedTransform.position, GetDestinationPoint()) >= attackDistance)
        {
            Escape();
            SetState(State.Move);
        }
        else
        {
            Debug.Log("too close");
        }

    }
    protected override void Die()
    {
        animator.SetTrigger(isDeadTriggerName);
    }

    private void CheckForSecondClick()
    {
        firstClickTime = Time.time;
        StartCoroutine(OnDoubleClick());
    }

    private IEnumerator OnDoubleClick()
    {
        isCoroutineDenied = true;
        while (Time.time < firstClickTime + timeBetweenClicks)
        {
            if (clickCounter == 2)
            {
                animator.SetBool(isRunningBoolName, true);
                navMeshAgent.speed = runningSpeed;
            }

            yield return new WaitForEndOfFrame();
        }

        clickCounter = 0;
        firstClickTime = 0f;
        isCoroutineDenied = false;
    }

    private void CheckForEquippedWeapon(bool isPlayerAgressive)
    {
        if (ep.EquipmentSlots[EquipmentSlotsID.Weapon1EquipmentSlotID].Item == null &&
            ep.EquipmentSlots[EquipmentSlotsID.Weapon2EquipmentSlotID].Item != null)
        {
            playerWeapon2.SetActive(isPlayerAgressive);
        }
        else if (ep.EquipmentSlots[EquipmentSlotsID.Weapon1EquipmentSlotID].Item != null &&
            ep.EquipmentSlots[EquipmentSlotsID.Weapon2EquipmentSlotID].Item == null)
        {
            playerWeapon1.SetActive(isPlayerAgressive);
        }
        else if (ep.EquipmentSlots[EquipmentSlotsID.Weapon1EquipmentSlotID].Item == null &&
            ep.EquipmentSlots[EquipmentSlotsID.Weapon2EquipmentSlotID].Item == null)
        {
            Debug.Log("Find a weapon");
        }
        else if (ep.EquipmentSlots[EquipmentSlotsID.Weapon1EquipmentSlotID].Item != null &&
            ep.EquipmentSlots[EquipmentSlotsID.Weapon2EquipmentSlotID].Item != null)
        {
            bonusWeapon.SetActive(isPlayerAgressive);
            playerWeapon2.SetActive(isPlayerAgressive);
        }
    }

    private void UpdateCurrentCondition()
    {
        maxHealth = inventoryManager.Vitality.GetFinalValue();
        maxStamina = inventoryManager.Agility.GetFinalValue();
    }

    private float CalculateStaminaExhaustion()
    {
        return 25f/inventoryManager.Agility.GetFinalValue();
    }

    private IEnumerator OnEscape()
    {
        yield return new WaitForSeconds(escapeTime);

        IsEscaping = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, destinationInfo.point);
        Gizmos.DrawSphere(destinationInfo.point, 0.1f);
    }

    private void CameraRaycast_OnPlayerMove(Vector3 endPosition)
    {
        if (isAlive && !isTired)
        {
            destinationInfo.point = endPosition;
            SetState(State.Move);
        }
    }

    private void CameraRaycast_OnPlayerStop()
    {
        SetState(State.Idle);
    }

    private void CameraRaycast_OnPlayerAttack(Vector3 endPosition)
    {
        destinationInfo.point = endPosition;
        if (enemyFinder.ClosestBandit != null)
        {
            navMeshAgent.SetDestination(enemyFinder.ClosestBandit.CachedTransform.position);
        }

    }

    private void CameraRaycast_OnPlayerInteract(Vector3 endPosition)
    {
        destinationInfo.point = endPosition;

        if (isAlive)
        {
            isCollecting = true;
            destinationInfo.point = endPosition;
            SetState(State.Move);
        }
    }

}
