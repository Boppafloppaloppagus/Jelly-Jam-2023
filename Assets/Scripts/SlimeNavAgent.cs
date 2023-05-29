using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//this script has been poorly named and is now unofficially the SlimeEverythingController.

/*
 * when the player is within the volume follow the player but never leave the volume 
 * if the next volume is outside the sphere stay in the current one???? 
 * sit still while player is looking if time allows, else just leave the zone
 * only throw nav agent and fruit
 * do the raycast pet and /playanim or whatever the hell.
 * stop pucntuating comments with semi colons;
 */

public class SlimeNavAgent : MonoBehaviour
{
    public GameObject playerStuff;
    public GameObject ballStuff;
    public GameObject foodStuff;
    public GameObject home;
    public SphereCollider jellyInteractRadiusContainer;
    public SphereCollider playerInteractRadiusContainer;
    public SphereCollider theZone;

    public Renderer jellySkin;


    public Transform[] palmTrees;
    private Vector3 withoutThatStupidY;
    private Vector3 randomPoint;
    private Vector3 goTo;
    private Vector3 startPoint;
    private Vector3 dangerZone;

    //other scripts
    private ThrowController throwController;
    private FoodChecker foodChecker;
    public GameController gameController;

    NavMeshAgent agent;

    //public bool takeAnL;
    bool fetchStart;
    bool carryingSomething;
    bool foodStart;
    bool playerInZone;
    bool petStart;
    bool haveIBeenBeaned;
    bool haveIBeenCalled;
    bool pointSet;


    float waitASec;
    float wait;
    float timer;

    enum State
    {
        MuckAbout,
        Fetch,
        Food,
        Pet,
        Called,
        CommitDie
    }

    State slimeState;
    // Start is called before the first frame update
    void Start()
    {
        //just gettin some components and stuff
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        throwController = playerStuff.GetComponent<ThrowController>();
        foodChecker = jellyInteractRadiusContainer.GetComponent<FoodChecker>();
        //Set Initial destination for jelly
        randomPoint = Random.insideUnitSphere * 3 + home.transform.position;
        //this is used later to prevent the jelly from just 
        //grabbing the ball out of the air when its tossed.
        waitASec = 1;

        startPoint = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(slimeState);
        CheckForFetch();
        CheckForFood();
        CheckForPetTime();
        CheckForCall();
        IsPlayerInZone();
        SetState();
        
        switch (slimeState)
        {
            case State.MuckAbout:
                MuckAbout();
                //jellySkin.material.SetColor("_Color", new Color(1, 1, 1, 1));
                break;
            case State.Fetch:
                Fetch();
                break;
            case State.Food:
                Eat();
                break;
            case State.Pet:
                GetPetNerd();
                break;
            case State.Called:
                Called();
                break;
            case State.CommitDie:
                //GoCommitDie();
                break;
        }

    }

    //methods associated with states
    void MuckAbout()
    {
        agent.enabled = true;//may need to removes these
        // pointSet = false;
        //Debug.Log(randomPoint);
        if (Vector3.Distance(this.transform.position, randomPoint) > 1)
        {
            agent.destination = randomPoint;
            wait = 5f;
        }
        else
        {
            if (wait > 0)
            {
                wait -= Time.deltaTime;
            }
            else
            {
                randomPoint = Random.insideUnitSphere * 3 + home.transform.position;
                randomPoint = new Vector3(randomPoint.x, 0, randomPoint.z);
            }
        }

    }

    void Eat()
    {
        agent.enabled = true;//may need to removes these
        if (Vector3.Distance(this.transform.position, foodStuff.transform.position) > 1)
            agent.destination = foodStuff.transform.position;
        else
        {
            foodStuff.SetActive(false);
            foodStart = false;
        }
    }

    void Fetch()
    {
        agent.enabled = true;//may need to removes these
        ballStuff = throwController.interactableObject;
        if (!theZone.bounds.Contains(ballStuff.transform.position))
        {
            waitASec = 1;
            carryingSomething = false;
            fetchStart = false;
        }

        if (waitASec > 0 && throwController.holdingSomethingFetchable)
        {
            agent.destination = playerInteractRadiusContainer.ClosestPoint(this.transform.position);
        }
        else
        {
            waitASec -= Time.deltaTime;
        }
        if (!carryingSomething && waitASec <= 0 && Vector3.Distance(this.transform.position, ballStuff.transform.position) > 2)
        {
            agent.destination = ballStuff.transform.position;
        }
        else if (waitASec <= 0)
        {
            carryingSomething = true;
            ballStuff.transform.localPosition = this.transform.position + new Vector3(0, 1, 0);
        }
        if (carryingSomething && Vector3.Distance(this.transform.position, playerStuff.transform.position) > 5)
        {
            agent.destination = playerInteractRadiusContainer.ClosestPoint(this.transform.position);
        }
        else if (carryingSomething || throwController.holdingJelly)
        {
            waitASec = 1;
            carryingSomething = false;
            fetchStart = false;
            Vector3 forceToAdd = this.transform.forward * 5 + transform.up * 5;
            ballStuff.GetComponent<Rigidbody>().AddForce(forceToAdd, ForceMode.Impulse);
        }
        //If the player is still holding the ball I want to follow him, otherwise if he's released the ball and I don't have it I want to go grab it

    }

    void GetPetNerd()
    {
        Debug.Log("Congrats, you've pet the cube.");
        petStart = false;
        //do stuff 
    }

    void Called()
    {
        if (!playerInteractRadiusContainer.bounds.Contains(this.transform.position))
        {
            agent.destination = playerInteractRadiusContainer.ClosestPoint(this.transform.position);
            wait = 1.5f;
        }
        else if (wait > 0)
        {
            wait -= Time.deltaTime;
        }
        else
        {
            haveIBeenCalled = false;
        }

    }

    //----------------------------------------------------------------------------------------Past this line thar be checking for things
    //I need this for when the player throws the ball and the fetch state isn't actually over
    void CheckForFetch()
    {
        if (throwController.holdingSomethingFetchable && theZone.bounds.Contains(this.transform.position) && theZone.bounds.Contains(playerStuff.transform.position))
        {
            fetchStart = true;
        }
    }

    void CheckForFood()
    {
        if (foodChecker.isFood == true)
        {
            foodStuff = foodChecker.maybeFood;
            foodStart = true;
        }
    }

    void CheckForPetTime()
    {
        if (throwController.petTimeNow)
            petStart = true;
    }

    void CheckForCall()
    {
        if (throwController.calledJelly)
            haveIBeenCalled = true;

    }

    void IsPlayerInZone()
    {
        if (!theZone.bounds.Contains(playerStuff.transform.position))
        {
            playerInZone = false;
            waitASec = 1;
            carryingSomething = false;
            fetchStart = false;
        }
        else
            playerInZone = true;

    }


    //-----------------------------------------------------------------------------------------------checking for things ends here


    void SetState()
    {
        if (haveIBeenCalled)
            slimeState = State.Called;
        else if (foodStart && playerInZone && theZone.bounds.Contains(this.transform.position))
            slimeState = State.Food;
        else if (fetchStart && playerInZone && theZone.bounds.Contains(this.transform.position))
            slimeState = State.Fetch;
        else if (petStart)
            slimeState = State.Pet;
        /*else if (!playerInZone || pointSet == true && !theZone.bounds.Contains(this.transform.position))
            slimeState = State.CommitDie; //change this later - changed*/
        else if (playerInZone && theZone.bounds.Contains(this.transform.position))
            slimeState = State.MuckAbout;

    }
    //past ere be old stuff me wanted saved.
    /*
    void GoCommitDie()
    {
        if (throwController.holdingJelly)
        {
            
            dangerZone = palmTrees[0].position;
            Debug.Log(palmTrees.Length);
            for (int i = 1; i < palmTrees.Length; i++)
            {
                if (Vector3.Distance(palmTrees[i].position, this.transform.position) < Vector3.Distance(dangerZone, this.transform.position))
                    dangerZone = palmTrees[i].position;
                Debug.Log(dangerZone);
            }
            agent.destination = dangerZone;
            pointSet = true;
        }
        else if (!pointSet && !throwController.holdingJelly)
        {
            int index = Random.Range(0, 3);
            dangerZone = new Vector3(palmTrees[index].position.x, 0, palmTrees[index].position.z);
            agent.destination = dangerZone;
            pointSet = true;
        }
        else if (Vector3.Distance(this.transform.position, agent.destination) <= 2 && !throwController.holdingJelly)
        {
            jellySkin.material.SetColor("_Color", new Color(1, 0, 0, 1));
            agent.enabled = false;
            pointSet = false;
            takeAnL = true;
        }
    }
    */

}
