using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingState : CharacterState
{
    bool m_attackIsCompleted = false;
    public override void OnEnter()
    {
        Debug.Log("Enter state: AttackingState\n");
        m_stateMachine.Animator.SetTrigger("Attack");
        m_attackIsCompleted=true;
    }
    public override void OnExit()
    {
        Debug.Log("Exit state: AttackingState\n");
    }


    public override bool CanEnter()
    {
        //This must be run in Update absolutely
        return Input.GetMouseButtonDown(0) && m_stateMachine.IsInState<FreeState>();
    }

    public override bool CanExit()
    {
        return m_attackIsCompleted;
    }
}
