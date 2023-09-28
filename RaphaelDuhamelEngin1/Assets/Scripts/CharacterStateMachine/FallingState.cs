using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : CharacterState
{
    private bool m_isFalling = true;
    private Vector3 m_fallStartPoint;
    private float m_fallDistance;
    public override void OnEnter()
    {
        Debug.Log("Enter state: FallingState\n");
        m_stateMachine.Animator.SetTrigger("Falling");
        m_stateMachine.Animator.SetBool("Falling",true);
        m_fallStartPoint = m_stateMachine.transform.position;
    }
    public override void OnExit()
    {
        Debug.Log("Exit state: FallingState\n");
    }

    public override void OnUpdate()
    {
        bool isGrounded = m_stateMachine.IsInContactWithFloor();

        if (isGrounded)
        {
            m_isFalling = false;
            m_fallDistance = m_fallStartPoint.y - m_stateMachine.transform.position.y;

            if (m_fallDistance > 5f)
            {
                m_stateMachine.m_isStunned = true;
            }
            else
            {
                m_stateMachine.Animator.SetBool("Falling", false);

            }
        }
    }

    public override bool CanEnter()
    {
        //This must be run in Update absolutely
        return !m_stateMachine.IsInContactWithFloor() && m_stateMachine.IsInState<FreeState>();
    }

    public override bool CanExit()
    {
        return !m_isFalling;
    }
}
