using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Archer : BaseBandit
{
    protected override void Update()
    {
        base.Update();

        stateMachine.UpdateState();

    }
}
