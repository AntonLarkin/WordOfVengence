using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bandit : MonoBehaviour
{
    private enum State
    {
        Idle,
        Chase,
        Attack,
        Return,
        Die
    }

    [Header("NavMeshAgent")]
    [SerializeField] private LayerMask accesableArea;
    [SerializeField] private LayerMask unaccesableArea;
    [SerializeField] private float runningSpeed;
    [SerializeField] private float walkingSpeed;
    private NavMeshAgent navMeshAgent;
    private const float minDistance = 3f;

    [Header("Animation")]
    [SerializeField] private Animator banditAnimator;
    [SerializeField] private string isPatrolingBoolName;
    [SerializeField] private string isPlayerFoundBoolName;
    [SerializeField] private string attackSimpleTriggerName;
    [SerializeField] private string isFollowingBoolName;

    [Header("Areas")]
    [SerializeField] private float noticeRadius;
    [SerializeField] private float chaseRadius;
    [SerializeField] private float attackRadius;

    [Header("Player")]
    [SerializeField] private Player player;

    [Header("Patroling")]
    [SerializeField] private bool isPatroling;
    [SerializeField] private Transform[] patrolPointsTransform;
    private Transform patrolPositionTransform;

    private bool isMoving;
    private bool isAttacking;

    private StateMachine stateMachine;

    public LayerMask AccesableArea => accesableArea;
    public LayerMask UnccesableArea => unaccesableArea;
    public float RunningSpeed => runningSpeed;
    public float WalkingSpeed => walkingSpeed;
    public NavMeshAgent NavMeshAgent => navMeshAgent;
    public float MinDistance => minDistance;

    public bool IsPatroling => isPatroling;
    public Transform[] PatrolPointsTransform => patrolPointsTransform;
    public Transform PatrolPositionTransform => patrolPositionTransform;

    public float NoticeRadius => noticeRadius;
    public float ChaseRadius => chaseRadius;
    public float AttackRadius => attackRadius;

    public Animator BanditAnimator => banditAnimator;
    public string IsPatrolingBoolName => isPatrolingBoolName;
    public string IsPlayerFoundBoolName => isPlayerFoundBoolName;
    public string AttackSimpleTriggerName => attackSimpleTriggerName;
    public string IsFollowingBoolName => isFollowingBoolName;

    public Transform CachedTransform { get; private set; }
    public Transform PlayerTransform { get; private set; }

    private void Awake()
    {
        stateMachine = new StateMachine();

        CachedTransform = transform;
        //patrolPositionTransform = CachedTransform;
        PlayerTransform = player.transform;
    }

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        stateMachine.SetState(new IdleState(this, stateMachine));
    }

    private void Update()
    {
        stateMachine.UpdateState();

        CheckState();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, noticeRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }

    private void CheckState()
    {
        var playerPosition = PlayerTransform.position;
        var distance = Vector3.Distance(transform.position, playerPosition);

        if (distance < noticeRadius&&distance>attackRadius)
        {
            Debug.Log("notice");
        }
        else if (distance < attackRadius)
        {
            Debug.Log("Attack");
        }
        else if (distance > noticeRadius && distance < chaseRadius)
        {
            Debug.Log("Chase");
        }

    }
}
