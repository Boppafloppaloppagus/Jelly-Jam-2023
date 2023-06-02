using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VRPettingInteraction : MonoBehaviour
{
    [SerializeField] Transform leftController;
    [SerializeField] Transform rightController;
    public Animator jellyAnimator;

    private Vector3 controllerPosition1;
    private Vector3 previousControllerPosition1;

    private Vector3 controllerPosition2;
    private Vector3 previousControllerPosition2;

    public Vector3 triggerStartPosition;
    public Vector3 triggerSize;
    [SerializeField] bool handInside;
    [SerializeField]bool controller1;
    [SerializeField]bool controller2;
    // This is a threshold to determine if the controller is moving
    public float movementThreshold = 0.01f;


    void Update()
    {
        bool leftH = false;
        bool rightH = false;

        previousControllerPosition1 = controllerPosition1;
        controllerPosition1 = leftController.position;

        if (handInside)
        {
            Debug.Log("handInside");
        }


        if (handInside && IsControllerMoving(controllerPosition1, previousControllerPosition1))
        {
            leftH = true;
        }

        previousControllerPosition2 = controllerPosition2;
        controllerPosition2 = rightController.position;

        if (handInside && IsControllerMoving(controllerPosition2, previousControllerPosition2))
        {
            rightH = true;
           
        }
        if (rightH || leftH)
        {
            StartPet(true);
        }else
        {
            StartPet(false);
        }

    }
    void StartPet(bool value)
    {
        jellyAnimator.SetBool("Pet", value);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            handInside = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            handInside = false;
        }
    }
    private bool IsControllerMoving(Vector3 currentPos, Vector3 previousPos)
    {
        // Check if the controller has moved a certain distance since the last frame
        return (currentPos - previousPos).magnitude > movementThreshold;
    }
    void OnDrawGizmos()
    {
        // Draw a cube in the Scene view at the trigger zone position with the trigger zone size
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.TransformPoint(triggerStartPosition), triggerSize);
    }
}
