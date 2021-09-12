using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Archer : BaseBandit
{

    [SerializeField] private Transform arrowSpawnPointTransform;
    [SerializeField] private Arrow arrowPrephab;
    [SerializeField] private float delayTime;

    private bool isActiveShooting;

    protected override void Update()
    {
        base.Update();

        stateMachine.UpdateState();

        if (isShooting && !isActiveShooting)
        {
            isActiveShooting = true;
            CreateArrow();
        }
        
    }

    private void CreateArrow()
    {
        Instantiate(arrowPrephab, arrowSpawnPointTransform);
        StartCoroutine(OnShoot());
    }

    private IEnumerator OnShoot()
    {
        yield return new WaitForSeconds(delayTime);

        isActiveShooting = false;
    }

}
