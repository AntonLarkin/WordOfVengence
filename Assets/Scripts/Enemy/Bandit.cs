using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bandit : BaseBandit
{
    protected override void Update()
    {
        base.Update();

        PlayerTransform.position = GetPlayerPosition();

    }

    //private void ondrawgizmos()
    //{
    //    gizmos.color = color.blue;
    //    gizmos.drawwiresphere(transform.position, noticeradius);

    //    gizmos.color = color.red;
    //    gizmos.drawwiresphere(transform.position, attackradius);

    //    gizmos.color = color.cyan;
    //    gizmos.drawwiresphere(transform.position, chaseradius);
    //}

    private Vector3 GetPlayerPosition()
    {
        return PlayerTransform.position;
    }
}
