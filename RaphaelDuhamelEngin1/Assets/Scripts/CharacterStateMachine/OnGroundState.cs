using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundState : CharacterState
{
    private const float STATE_EXIT_TIMER = 1.0f;
    private float m_currentStateTimer = 0.0f;
    public override void OnEnter()
    {
        Debug.Log("Enter state: OnGroundState\n");
        m_stateMachine.Animator.SetBool("Falling", false);
        if (!m_stateMachine.m_isGrounded)
        {
            m_stateMachine.Animator.SetTrigger("Stunned");
        }
        m_currentStateTimer = STATE_EXIT_TIMER;
    }
    public override void OnExit()
    {
        m_stateMachine.m_isGrounded = false;
        m_stateMachine.m_isStunned = false;
        m_stateMachine.m_isGettingUp = true;
        Debug.Log("Exit state: OnGroundState\n");
    }

    public override void OnUpdate()
    {
        m_currentStateTimer -= Time.deltaTime;
    }

    public override bool CanEnter()
    {
        //This must be run in Update absolutely
        return m_stateMachine.m_isStunned || m_stateMachine.m_isGrounded;
    }
    public override bool CanExit()
    {
        return m_currentStateTimer <= 0;
    }
}
