using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowController : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectThrow;

    [Header("Settings")]
    public float throwCooldown;

    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.Mouse0;
    public KeyCode dropKey = KeyCode.Mouse1;
    public float throwForce;
    public float throwUpwardForce;

    bool readyToThrow;


    //Boppa Zone------------------------------------------------------
    //I want to add the players rb velocity to the throw
    public Rigidbody playerRb;
    //I only want to be able to pick things up if they're close enough
    public float pickupRange;
    //I want to know what I can interact with
    public LayerMask interactableObjectMask;
    private RaycastHit interactableInfo;
    private GameObject interactableObjectInView;
    public GameObject interactableObject;
    Renderer objectRenderer;
    //making sure I can do things without other things freaking out
    bool seenAnIneractable;
    bool holdingSomething;
    bool setNewHeldObject;
    //this is being used by the nav agent to start the fetching mechanic
    public bool holdingSomethingFetchable;
    public bool holdingJelly;
    //----------------------------------------------------------------
    public bool petTimeNow;

    private void Start()
    {
        //readyToThrow = true;
    }

    private void Update()
    {
        FindPickup();
        if (holdingSomething && Input.GetKeyDown(throwKey)) //&& readyToThrow)
        {
            Throw();
        }
        else if ( Input.GetKeyDown(throwKey)) //&& readyToThrow)
        {
            Pickup();
        }
        if (holdingSomething && Input.GetKeyDown(dropKey))
        {
            holdingSomething = false;
            holdingJelly = false;
        }
        else if (Input.GetKeyDown(dropKey))
        {
            PetTheGoodBoi();
        }
        else
        {
            petTimeNow = false;
        }

        HoldObject();
    }
    /*
     * 
     * 
     * 
     *
     * Start by checking the value of the forcedirection property at different camera angles
     * 
     * 
     * 
     * 
     */
    private void Throw()
    {
        //readyToThrow = false;
        //-----------------------------------------------------
        holdingSomething = false;
        holdingSomethingFetchable = false; 
        holdingJelly = false;
        //--------------------------------------------------

        GameObject projectile = interactableObject;//Instantiate(objectThrow, attackPoint.position, cam.rotation);

        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        Vector3 forceDirection = cam.transform.forward;

  
        /*
        RaycastHit hit;
        
        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }
        */
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce + playerRb.velocity;
        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        //Invoke(nameof(ResetThrows), throwCooldown);

    }
    /*
    private void ResetThrows()
    {
        readyToThrow = true;
    }
    */
    //----------------------------
    //This is for finding interactable objects
    private void FindPickup()
    {
        if (Physics.Raycast(cam.position, cam.forward, out interactableInfo, pickupRange, interactableObjectMask) && !seenAnIneractable)
        {
            seenAnIneractable = true;
            //Now I know about what it is
            interactableObjectInView = interactableInfo.transform.gameObject;


            interactableObject = interactableObjectInView;

            //Highlighting the object for users sake
            if (!holdingSomething)
            {
                objectRenderer = interactableObjectInView.GetComponent<Renderer>();

                objectRenderer.material.SetColor("_Color", new Color(0, 1, 0, 1));
            }


        }
        //Stop Highlighting
        else if(!Physics.Raycast(cam.position, cam.forward, out interactableInfo, pickupRange, interactableObjectMask) && seenAnIneractable)
        {
            objectRenderer.material.SetColor("_Color", new Color(1, 1, 1, 1));
            seenAnIneractable = false;
            interactableObjectInView = null;
        }
    }
    void PetTheGoodBoi()
    {
        if (Physics.Raycast(cam.position, cam.forward, out interactableInfo, pickupRange, interactableObjectMask) && interactableInfo.collider.gameObject.tag == "Jelly")
        {
            petTimeNow = true;
        }
    }
    //This is to signal whether or not the interactable objects transform should have its transform tied to the player
    private void Pickup()
    { 
        if (Physics.Raycast(cam.position, cam.forward, out interactableInfo, pickupRange, interactableObjectMask))
            holdingSomething = true;
    }

    private void HoldObject()
    {
        if (holdingSomething)
        {
            interactableObject.transform.position = attackPoint.transform.position;
            interactableObject.transform.rotation = attackPoint.transform.rotation;
            if (interactableObject.tag == "Fetchable")
                holdingSomethingFetchable = true;
            else if (interactableObject.tag == "Jelly")
            {
                holdingJelly = true;
            }
        }
        else if(interactableObjectInView = null)
        {
            interactableObject = null;
        }
    }
    //----------------------------
}
