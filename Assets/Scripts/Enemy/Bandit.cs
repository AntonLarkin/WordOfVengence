using System;
using System.Collections.Generic;
using UnityEngine;

public class Bandit : MonoBehaviour
{
    private Dictionary<Type, IEnemyBehaviour> behaviourMap;
    private IEnemyBehaviour behaviourCurrent;

    private void Start()
    {
        InitBehaviour();
        SetBehaviourIdle();
    }

    public void SetBehaviourIdle()
    {
        var behaviour = GetBehaviour<BanditBehaviourIdle>();
        SetBehaviour(behaviour);
    }

    public void SetBehaviourActive()
    {
        var behaviour = GetBehaviour<BanditBehaviourActive>();
        SetBehaviour(behaviour);
    }

    public void SetBehaviourAttack()
    {
        var behaviour = GetBehaviour<BanditBehaviourAttack>();
        SetBehaviour(behaviour);
    }

    private void InitBehaviour()
    {
        behaviourMap = new Dictionary<Type, IEnemyBehaviour>();

        behaviourMap[typeof(BanditBehaviourIdle)] = new BanditBehaviourIdle();
        behaviourMap[typeof(BanditBehaviourActive)] = new BanditBehaviourActive();
        behaviourMap[typeof(BanditBehaviourAttack)] = new BanditBehaviourAttack();
    }

    private void SetBehaviour(IEnemyBehaviour newBehaviour)
    {
        if (behaviourCurrent != null)
        {
            behaviourCurrent.Exit();
        }

        behaviourCurrent = newBehaviour;
        behaviourCurrent.Enter();
    }

    private IEnemyBehaviour GetBehaviour<T>() where T : IEnemyBehaviour
    {
        var type = typeof(T);
        return behaviourMap[type];
    }

    private void Update()
    {
        if (behaviourCurrent != null)
        {
            behaviourCurrent.Update();
        }
    }

}
