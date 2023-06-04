using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodChecker : MonoBehaviour
{
    public GameObject maybeFood;
    public bool isFood;
    public SphereCollider thisCollider;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       if (maybeFood != null && !thisCollider.bounds.Contains(maybeFood.transform.position))
            isFood = false;
    }
    void OnTriggerEnter(Collider collision)
    {
        maybeFood = collision.gameObject;

        if (collision.gameObject.tag == "Food")
            isFood = true;
        else
            isFood = false;
    }
}
