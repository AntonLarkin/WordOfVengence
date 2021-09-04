using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : BaseHuman
{
    [SerializeField] private CameraRaycast cameraRaycast;

    [Header("EnemyTrigger")]
    [SerializeField] private float triggerDistance;
    [SerializeField] private float attackDistance;
    [SerializeField] private GameObject playerWeapon;
    [SerializeField] private float escapeTime;
    private BaseBandit[] bandits;
    private BaseBandit currentBandit;

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
    private PlayerStateMachine stateMachine;

    public Animator Animator => animator; 
    public string IsWalkingBoolName => isWalkingBoolName;
    public string IsRunningBoolName => isRunningBoolName;
    public string EnemyNearbyTriggerName => enemyNearbyTriggerName;
    public string AttackTriggerName => attackTriggerName;
    public string IsAttackingBoolName => isAttackingBoolName;
    public string IsDeadTriggerName => isDeadTriggerName;
    public float WalkingSpeed=> walkingSpeed;
    public NavMeshAgent NavMeshAgent  => navMeshAgent;
    public float MinDistance => minDistance;
    public float AttackDistance => attackDistance;
    public GameObject PlayerWapon => playerWeapon;
    public Transform CachedTransform { get; private set; }
    public bool IsAgressive { get; private set; }
    public bool IsEscaping { get; private set; }
    public BaseBandit CurrentBandit => currentBandit;


    private void Awake()
    {
        CachedTransform = transform;
        cameraRaycast = FindObjectOfType<CameraRaycast>();
        bandits = FindObjectsOfType<BaseBandit>();
        stateMachine = new PlayerStateMachine();
    }

    private void OnEnable()
    {
        cameraRaycast.OnPlayerMove += CameraRaycast_OnPlayerMove;
        cameraRaycast.OnPlayerStop += CameraRaycast_OnPlayerStop;
        cameraRaycast.OnPlayerAttack += CameraRaycast_OnPlayerAttack;
    }

    private void OnDisable()
    {
        cameraRaycast.OnPlayerMove -= CameraRaycast_OnPlayerMove;
        cameraRaycast.OnPlayerStop -= CameraRaycast_OnPlayerStop;
        cameraRaycast.OnPlayerAttack -= CameraRaycast_OnPlayerAttack;
    }

    protected override void Start()
    {
        base.Start();

        destinationInfo.point = transform.position;
        isAlive = true;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

 
        stateMachine.SetState(new PlayerIdleState(this, stateMachine));

    }

    private void Update()
    {
        Debug.Log(stateMachine.ShowCurrentsState());
        if (CurrentHealth <= 0&&isAlive)
        {
            Die();
            isAlive = false;
        }

        CheckForEnemies();

        if (Input.GetMouseButtonUp(0))
        {
            clickCounter++;
        }

        if (clickCounter == 1 && !isCoroutineDenied)
        {
            CheckForSecondClick();
        }

        stateMachine.UpdateState();
    }

    public Vector3 GetDestinationPoint()
    {
        return destinationInfo.point;
    }

    public void SetArgessive(bool isAgresive)
    {
        IsAgressive = isAgresive;
    }

    public void Escape()
    {
        IsEscaping = true;
        StartCoroutine(OnEscape());
    }

    private void CheckForEnemies()
    {
        if (bandits.Length > 0)
        {
            foreach (BaseBandit bandit in bandits)
            {
                if (Vector3.Distance(transform.position, bandit.transform.position) <= triggerDistance)
                {
                    if (Vector3.Distance(transform.position, bandit.transform.position) <= attackDistance && !IsEscaping)
                    {
                        SetArgessive(true);
                        Debug.Log("trying to escape");
                        currentBandit = bandit;
                        if (currentBandit.CurrentHealth > 0)
                        {
                            Debug.Log(CurrentBandit);
                            stateMachine.SetState(new PlayerAttackState(this, stateMachine));
                        }
                        else
                        {
                            currentBandit = null;
                        }

                    }
                    else
                    {
                        SetArgessive(true);
                        stateMachine.SetState(new PlayerAgressiveState(this, stateMachine));
                    }

                }
                else
                {
                    stateMachine.SetState(new PlayerMovingState(this, stateMachine));
                    SetArgessive(false);
                }
            }
        }

    }

    protected override void Die()
    {
        stateMachine.SetState(new PlayerDieState(this, stateMachine));
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
        destinationInfo.point = endPosition;
        stateMachine.SetState(new PlayerMovingState(this, stateMachine));
    }

    private void CameraRaycast_OnPlayerStop()
    {
        stateMachine.SetState(new PlayerIdleState(this, stateMachine));
    }

    private void CameraRaycast_OnPlayerAttack(Vector3 endPosition)
    {
        destinationInfo.point = endPosition;
        navMeshAgent.SetDestination(destinationInfo.point);
        stateMachine.SetState(new PlayerMovingState(this, stateMachine));
    }

}
