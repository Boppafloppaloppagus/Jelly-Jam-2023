using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryResetti : MonoBehaviour
{
    public GameObject mahBerries;
    Vector3 whereMyBerriesGo;
    Rigidbody berryRb;
    float timer;
    public float spawnTime;

    // Start is called before the first frame update
    void Start()
    {
        whereMyBerriesGo = mahBerries.transform.position;
        berryRb = mahBerries.GetComponent<Rigidbody>();
        timer = spawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (mahBerries.activeSelf == false)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                mahBerries.transform.position = whereMyBerriesGo;
                berryRb.velocity = new Vector3(0, 0, 0);
                timer = spawnTime;
                mahBerries.SetActive(true);
            }
        }
    }
}
