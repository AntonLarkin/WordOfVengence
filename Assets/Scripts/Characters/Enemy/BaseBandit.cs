using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseBandit : BaseHuman
{
    [SerializeField] protected bool isMelee;
    [SerializeField] protected bool isLongRanged;

    [Header("NavMeshAgent")]
    [SerializeField] protected LayerMask accesableArea;
    [SerializeField] protected LayerMask unaccesableArea;
    [SerializeField] protected float runningSpeed;
    [SerializeField] protected float walkingSpeed;
    protected NavMeshAgent navMeshAgent;
    protected const float minDistance = 2f;

    [Header("Animation")]
    [SerializeField] protected Animator banditAnimator;
    [SerializeField] protected string isPatrolingBoolName;
    [SerializeField] protected string isFollowingBoolName;
    [SerializeField] protected string isPlayerFoundBoolName;
    [SerializeField] protected string attackSimpleTriggerName;
    [SerializeField] protected string isDeadTriggerName;

    [Header("Areas")]
    [SerializeField] protected float noticeRadius;
    [SerializeField] protected float attackRadius;
    [SerializeField] protected float chaseRadius;

    [Header("Player")]
    [SerializeField] protected Player player;

    [Header("Patroling")]
    [SerializeField] protected bool isPatroling;
    [SerializeField] protected Transform[] patrolPointsTransform;
    protected Transform patrolPositionTransform;

    [Header("Weapon")]
    [SerializeField] protected Weapon banditWeapon;

    protected bool isShooting;
    protected bool isAlive;
    protected BanditStateMachine stateMachine;

    #region Properties

    public bool IsMelee => isMelee;
    public bool IsLongRanged => isLongRanged;

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
    public float AttackRadius => attackRadius;
    public float ChaseRadius => chaseRadius;

    public Animator BanditAnimator => banditAnimator;
    public string IsPatrolingBoolName => isPatrolingBoolName;
    public string IsPlayerFoundBoolName => isPlayerFoundBoolName;
    public string AttackSimpleTriggerName => attackSimpleTriggerName;
    public string IsFollowingBoolName => isFollowingBoolName;
    public string IsDeadTriggerName => isDeadTriggerName;

    public Weapon BanditWeapon => banditWeapon;

    public Player Player => player;
    public Transform CachedTransform { get; private set; }
    public Transform PlayerTransform { get; private set; }

    #endregion

    protected virtual void Awake()
    {
        stateMachine = new BanditStateMachine();

        CachedTransform = transform;
        PlayerTransform = player.transform;
    }

    protected override void Start()
    {
        base.Start();

        isAlive = true;
        navMeshAgent = GetComponent<NavMeshAgent>();

        stateMachine.SetState(new BanditIdleState(this, stateMachine));
    }

    protected virtual void Update()
    {
        stateMachine.UpdateState();

        if (CurrentHealth <= 0 && isAlive)
        {
            Die();
            isAlive = false;
        }

    }

    public void SetBanditShoot(bool isBanditShooting)
    {
        isShooting = isBanditShooting;
    }

    protected override void Die()
    {
        stateMachine.SetState(new BanditDieState(this, stateMachine));
    }

}
