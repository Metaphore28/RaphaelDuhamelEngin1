using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunInAirState : CharacterState
{
    public override void OnEnter()
    {
        m_stateMachine.Animator.SetTrigger("ExitJump");
        m_stateMachine.Animator.SetTrigger("StunInAir");
        Debug.Log("Enter state: StunInAirState\n");
    }
    public override void OnExit()
    {
        m_stateMachine.m_isStunInAir = false;
        m_stateMachine.Animator.SetBool("TouchGround", true);
        m_stateMachine.m_isGrounded = true;
        Debug.Log("Exit state: StunInAirState\n");
    }

    public override bool CanEnter()
    {
        return m_stateMachine.m_isStunInAir;
    }

    public override bool CanExit()
    {
        return m_stateMachine.IsInContactWithFloor();
    }
}
