using UnityEditor;
using UnityEngine;

public class JumpState : CharacterState
{
    private const float MAX_JUMP_HEIGHT = 5f;
    private const float STATE_EXIT_TIMER = 0.2f;
    private float m_currentStateTimer = 0.0f;
    private float m_jumpStartPoint;
    private float m_fallDistance;

    public override void OnEnter()
    {
        Debug.Log("Enter state: JumpState\n");

        //Effectuer le saut
        m_stateMachine.RB.AddForce(Vector3.up * m_stateMachine.JumpIntensity, ForceMode.Acceleration);
        m_stateMachine.Animator.SetTrigger("Jump");
        m_stateMachine.Animator.SetBool("TouchGround", false);

        m_jumpStartPoint = m_stateMachine.transform.position.y;
        m_currentStateTimer = STATE_EXIT_TIMER;

    }

    public override void OnExit()
    {
        if (m_fallDistance > MAX_JUMP_HEIGHT)
        {
            m_stateMachine.m_isStunned = true;
        }
        else if (m_stateMachine.m_isStunInAir)
        {
            return;
        }
        else if (m_fallDistance < MAX_JUMP_HEIGHT)
        {
            m_stateMachine.Animator.SetBool("TouchGround", true);
            m_stateMachine.Animator.SetTrigger("EndJump");
        }
        Debug.Log("Exit state: JumpState\n");
    }

    public override void OnFixedUpdate()
    {
        if (!m_stateMachine.IsInContactWithFloor())
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

            float airControlFactor = 0.5f; 
            Vector3 airControlForce = moveDirection * m_stateMachine.AirControlSpeed * airControlFactor;
            m_stateMachine.RB.AddForce(airControlForce, ForceMode.Acceleration);
        }
    }

    public override void OnUpdate()
    {
        m_currentStateTimer -= Time.deltaTime;
    }

    public override bool CanEnter()
    {
        //This must be run in Update absolutely
        return Input.GetKeyDown(KeyCode.Space);
    }

    public override bool CanExit()
    {
        m_fallDistance = m_jumpStartPoint - m_stateMachine.transform.position.y;

        return m_fallDistance > MAX_JUMP_HEIGHT || m_currentStateTimer <= 0;
    }
}
