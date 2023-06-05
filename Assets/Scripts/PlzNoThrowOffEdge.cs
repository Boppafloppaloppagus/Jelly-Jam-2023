using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlzNoThrowOffEdge : MonoBehaviour
{
    Vector3 startPoint;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        startPoint = this.transform.position;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y < -5)
        {
            this.transform.position = startPoint;
            rb.velocity = Vector3.zero;
        }
        if (this.transform.position.y > 50)
        {
            this.transform.position = startPoint;
            rb.velocity = Vector3.zero;
        }
    }
}
