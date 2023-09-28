using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTrigger : MonoBehaviour
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
                    characterStateMachine.m_isHit = true;
                }
            }
        }
    }
}
