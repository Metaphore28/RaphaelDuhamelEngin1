using UnityEngine;

public class StunTrigger : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Transform childTransform = collision.gameObject.transform.Find("CharacterController");

            if (childTransform != null)
            {
                CharacterControllerStateMachine characterStateMachine = childTransform.GetComponent<CharacterControllerStateMachine>();

                if (characterStateMachine != null)
                {
                    if (!characterStateMachine.IsInContactWithFloor())
                    {
                        characterStateMachine.m_isStunInAir = true;
                    }
                    else
                    {
                        characterStateMachine.m_isStunned = true;
                    }
                }
            }
        }
    }
}
