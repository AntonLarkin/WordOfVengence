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
    [SerializeField] private GameObject playerWeapon;
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
        if (other.CompareTag(Tags.Item)&&isCollecting)
        {
             Destroy(other.gameObject);
             isCollecting = false;
             SetState(State.Idle);
        }
    }

    private void Update()
    {
        if (isAlive)
        {
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
        playerWeapon.SetActive(isPlayerAgressive);
    }

    private void CheckState()
    {
        if (enemyFinder.ClosestBandit != null)
        {
            if (Vector3.Distance(CachedTransform.position, enemyFinder.ClosestBandit.transform.position) <= attackDistance&&!IsEscaping)
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
        else if (currentState == State.Move)
        {
            Move();
        }
        else if (currentState == State.Attack)
        {
            Attack();
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
                    Debug.Log("Idle");
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
        
        if(enemyFinder.ClosestBandit==null)
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
        if (isAlive)
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
