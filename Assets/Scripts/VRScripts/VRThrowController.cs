using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.XR;
public class VRThrowController : MonoBehaviour
{
    [HideInInspector]
    public bool isGrabbing;
    public GameObject interactableObject;
    public bool isHoldingJelly;
    public bool isHoldingSomethingFetchable;
    public bool petTimeNow;
    public bool calledJelly;
    public InputActionProperty callJelly;
    public AudioClip whistle;
    public AudioSource audioSource;

    private void Update()
    {
        float callJellyValue = callJelly.action.ReadValue<float>();
        if (callJellyValue > 0.8f)
        {
            calledJelly = true;
            if (!audioSource.isPlaying)
            {
                audioSource.clip = whistle;
                audioSource.Play();
            }
        }
        else
        {
            calledJelly = false;
        }
    }
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
