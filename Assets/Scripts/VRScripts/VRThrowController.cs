using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class VRThrowController : MonoBehaviour
{
    [HideInInspector]
    public bool isGrabbing;
    public GameObject interactableObject;
    public bool isHoldingJelly;
    public bool isHoldingSomethingFetchable;


    public void isPlayerGrabbing(bool value)
    {
        if (value)
        {
            isGrabbing = true;
        }
        else
        {
            isGrabbing = false;
            //interactableObject = null;
            isHoldingJelly = false;
            isHoldingSomethingFetchable = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fetchable")
        {
            isHoldingSomethingFetchable = true;
        }
        else if (other.tag == "Jelly")
        {
            isHoldingJelly = true;
        }
    }
}
