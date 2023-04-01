using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool smoothTransition = false;
    public float transitionSpeed = 10f;
    public float transitionRotationSpeed = 500f;

    Vector3 targetGridPosition;
    Vector3 prevTargetGridPosition;
    Vector3 targetRotation;

    private void Start()
    {
        targetGridPosition = Vector3Int.RoundToInt(transform.position);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
    
    void MovePlayer() 
    {
        if (Mathf.RoundToInt(targetGridPosition.x) % 2 == 0 || Mathf.RoundToInt(targetGridPosition.z) % 2 == 0)
        {
            prevTargetGridPosition = targetGridPosition;
            Vector3 targetPosition = targetGridPosition;

            if (targetRotation.y > 270f && targetRotation.y < 361f) targetRotation.y = 0f;
            if (targetRotation.y < 0f) targetRotation.y = 270f;

            if (!smoothTransition)
            {
                transform.position = targetPosition;
                transform.rotation = Quaternion.Euler(targetRotation);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * transitionSpeed);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * transitionRotationSpeed);
            }
        }
        else
        {
            targetGridPosition = prevTargetGridPosition;
        }
    }

    public void RotateLeft() { if (AtRest) targetRotation -= Vector3.up * 90f; }
    public void RotateRight() { if (AtRest) targetRotation += Vector3.up * 90f; }
    public void MoveForward() { if (AtRest) targetGridPosition += transform.forward; }
    public void MoveBackwards() { if (AtRest) targetGridPosition -= transform.forward; }
    public void MoveLeft() { if (AtRest) targetGridPosition -= transform.right; }
    public void MoveRight() { if (AtRest) targetGridPosition += transform.right; }


    bool AtRest {
        get {
            if ((Vector3.Distance(transform.position, targetGridPosition)) < 0.05f && 
                (Vector3.Distance(transform.eulerAngles, targetRotation) < 0.05f))
                return true;
            else
                return false;
        }
    }
}