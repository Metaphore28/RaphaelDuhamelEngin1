using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GettingUpState : CharacterState
{
    private const float STATE_EXIT_TIMER = 0.3f;
    private float m_currentStateTimer = 0.0f;
    public override void OnEnter()
    {
        Debug.Log("Enter state: GettingUpstate\n");
        m_stateMachine.Animator.SetTrigger("GettingUp");
        m_currentStateTimer = STATE_EXIT_TIMER;
    }
    public override void OnExit()
    {
        m_stateMachine.m_isGettingUp = false;
        Debug.Log("Exit state: GettingUpstate\n");
    }

    public override void OnUpdate()
    {
        m_currentStateTimer -= Time.deltaTime;
    }

    public override bool CanEnter()
    {
        //This must be run in Update absolutely
        return m_stateMachine.m_isGettingUp;
    }

    public override bool CanExit()
    {
        return m_currentStateTimer <= 0;
    }
}
