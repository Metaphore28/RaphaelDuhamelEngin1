using System;
using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform m_objectToLookAt;
    [SerializeField]
    private float m_rotationSpeed = 1.0f;
    [SerializeField]
    private Vector2 m_clampingXRotationValues = Vector2.zero;
    private Vector3 m_targetPosition;
    private float m_desiredPosition;
    public float m_lerpSpeed;
    public float m_camMinDistance;
    public float m_camMaxDistance;

    // Update is called once per frame
    void Update()
    {
        UpdateHorizontalMovements();
        UpdateVerticalMovements();
        UpdateCameraScroll();
    }

    private void FixedUpdate()
    {
        MoveCameraInFrontOfObstructionsFUpdate();
    }

    private void UpdateHorizontalMovements()
    {
        float currentAngleX = Input.GetAxis("Mouse X") * m_rotationSpeed;
        transform.RotateAround(m_objectToLookAt.position, m_objectToLookAt.up, currentAngleX);
    }

    private void UpdateVerticalMovements()
    {
        float currentAngleY = Input.GetAxis("Mouse Y") * m_rotationSpeed;
        float eulersAngleX = transform.rotation.eulerAngles.x;

        float comparisonAngle = eulersAngleX + currentAngleY;

        comparisonAngle = ClampAngle(comparisonAngle);

        if ((currentAngleY < 0 && comparisonAngle < m_clampingXRotationValues.x)
            || (currentAngleY > 0 && comparisonAngle > m_clampingXRotationValues.y))
        {
            return;
        }
        transform.RotateAround(m_objectToLookAt.position, transform.right, currentAngleY);
    }

    private void UpdateCameraScroll()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            m_desiredPosition += Input.mouseScrollDelta.y;
            m_targetPosition = transform.position + transform.forward * Input.mouseScrollDelta.y;

            float proposedDistance = Vector3.Distance(m_targetPosition, m_objectToLookAt.position);
            if (proposedDistance < m_camMinDistance)
            {
                m_targetPosition = transform.position + (m_targetPosition - transform.position).normalized * (m_camMinDistance - Vector3.Distance(transform.position, m_objectToLookAt.position));
            }
            else if (proposedDistance > m_camMaxDistance)
            {
                m_targetPosition = transform.position + (m_targetPosition - transform.position).normalized * (m_camMaxDistance - Vector3.Distance(transform.position, m_objectToLookAt.position));
            }
            transform.position = Vector3.Lerp(transform.position, m_targetPosition, m_lerpSpeed * Time.deltaTime);
        }
    }



    private void MoveCameraInFrontOfObstructionsFUpdate()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        RaycastHit hit;

        var vecteurDiff = transform.position - m_objectToLookAt.position;
        var distance = vecteurDiff.magnitude;

        if (Physics.Raycast(m_objectToLookAt.position, vecteurDiff, out hit, distance, layerMask))
        {
            //J'ai un objet entre mon focus et ma caméra
            Debug.DrawRay(m_objectToLookAt.position, vecteurDiff.normalized * hit.distance, Color.yellow);
            transform.SetPositionAndRotation(hit.point, transform.rotation);
        }
        else
        {
            //Je n'en ai pas
            Debug.DrawRay(m_objectToLookAt.position, vecteurDiff, Color.white);
            m_targetPosition = m_objectToLookAt.position + (vecteurDiff.normalized * m_desiredPosition);
            transform.SetPositionAndRotation(m_targetPosition, transform.rotation);
        }
    }

    private float ClampAngle(float angle)
    {
        if (angle > 180)
        {
            angle -= 360;
        }
        return angle;
    }
}
