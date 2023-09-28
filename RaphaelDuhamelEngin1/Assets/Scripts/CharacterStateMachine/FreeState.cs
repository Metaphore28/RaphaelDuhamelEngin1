using UnityEngine;

public class FreeState : CharacterState
{
    public override void OnEnter()
    {
        Debug.Log("Enter state: FreeState\n");
    }

    public override void OnFixedUpdate()
    {
        ApplyRelativeCameraMovements();
        ClampVelocities();
        ApplyDecelerationWhenNoInput();
        UpdateAnimatorValues();
    }

    private void ApplyRelativeCameraMovements()
    {
        Vector3 vectorOnFloor = Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.forward, Vector3.up).normalized;
        Vector3 rightDirection = m_stateMachine.Camera.transform.right;
        Vector3 backwardDirection = -vectorOnFloor;

        if (Input.GetKey(KeyCode.W))
            m_stateMachine.RB.AddForce(vectorOnFloor * m_stateMachine.AccelerationValue, ForceMode.Acceleration);

        if (Input.GetKey(KeyCode.A))
            m_stateMachine.RB.AddForce(-rightDirection * m_stateMachine.AccelerationValue, ForceMode.Acceleration);

        if (Input.GetKey(KeyCode.D))
            m_stateMachine.RB.AddForce(rightDirection * m_stateMachine.AccelerationValue, ForceMode.Acceleration);

        if (Input.GetKey(KeyCode.S))
            m_stateMachine.RB.AddForce(backwardDirection * m_stateMachine.AccelerationValue, ForceMode.Acceleration);
    }

    private void ClampVelocities()
    {
        Vector3 currentVelocity = m_stateMachine.RB.velocity;
        if (currentVelocity.magnitude > m_stateMachine.MaxVelocity)
        {
            currentVelocity = currentVelocity.normalized * m_stateMachine.MaxVelocity;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            currentVelocity = Vector3.ClampMagnitude(currentVelocity, m_stateMachine.MaxSideVelocity);
        }

        if (Input.GetKey(KeyCode.S))
        {
            currentVelocity = Vector3.ClampMagnitude(currentVelocity, m_stateMachine.MaxBackwardVelocity);
        }

        m_stateMachine.RB.velocity = currentVelocity;
    }

    private void ApplyDecelerationWhenNoInput()
    {
        if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S)))
        {
            m_stateMachine.RB.velocity -= m_stateMachine.RB.velocity.normalized * m_stateMachine.DecelerationValue * Time.fixedDeltaTime;
        }
    }


    private void UpdateAnimatorValues()
    {
        Vector3 forwardDirection = Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.forward, Vector3.up).normalized;
        Vector3 sideDirection = Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.right, Vector3.up).normalized;
        float forwardComponent = Vector3.Dot(m_stateMachine.RB.velocity, forwardDirection);
        float sideComponent = Vector3.Dot(m_stateMachine.RB.velocity, sideDirection);

        m_stateMachine.UpdateAnimatorValues(new Vector2(sideComponent, forwardComponent));
    }

    public override void OnExit()
    {
        Debug.Log("Exit state: FreeState\n");
    }

    public override bool CanEnter()
    {
        return m_stateMachine.IsInContactWithFloor();
    }

    public override bool CanExit()
    {
        return true;
    }
}
