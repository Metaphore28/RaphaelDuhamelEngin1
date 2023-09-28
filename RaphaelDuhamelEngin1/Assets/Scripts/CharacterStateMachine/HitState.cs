using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : CharacterState
{
    private const float STATE_EXIT_TIMER = 0.533f;
    private float m_currentStateTimer = 0.0f;

    public override void OnEnter()
    {
        Debug.Log("Enter state: HitState\n");

        m_stateMachine.Animator.SetTrigger("Hit");
        m_currentStateTimer = STATE_EXIT_TIMER;
    }
    public override void OnExit()
    {
        m_stateMachine.m_isHit = false;
        Debug.Log("Exit state: HitState\n");
    }

    public override void OnUpdate()
    {
        m_currentStateTimer -= Time.deltaTime;
    }

    public override bool CanEnter()
    {
        return m_stateMachine.m_isHit;
    }

    public override bool CanExit()
    {
        return m_currentStateTimer <= 0;
    }
}
