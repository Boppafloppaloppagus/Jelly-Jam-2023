using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{


    public SlimeNavAgent slimeScript;

    public GameObject[] trees;
    public GameObject[] berries;
    //public Text text;
    Vector3 berrieStartpos;

    public bool petTime, feedTime, fetchTime;

    //theres two phases, play and feed, feed needs to come after play, so beenFed or beenPet need to be true

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void textDisplay()
    { 
    }

    void GameOver()
    {
    }

    void JellyWantBerries()
    {
        //chooserandomcolor
    }

    void WhatTimeIsIt()
    {
        if (feedTime) 
        { 

        }
        else
        {
            if (Random.Range(0, 10) >= 5)
            {
                petTime = true;

            }
            else
            {
                fetchTime = true;
            }

        }
    }
    void JellyPlay()
    {
        
    }



    void ChangeBerriesLocation()
    {
        Vector3 transformPositionInHand;
        Vector3 transformRotationInHand;
        for (int i = 0; i < Random.Range(1,10); i++)
        {
            transformPositionInHand = trees[0].transform.position;
            transformRotationInHand = trees[0].transform.rotation.eulerAngles;

            for (int j = 0; j < trees.Length - 1; j++)
            {
                trees[j].transform.position = trees[j + 1].transform.position;
                trees[j].transform.eulerAngles = trees[j + 1].transform.rotation.eulerAngles;
            }
            trees[trees.Length - 1].transform.position = transformPositionInHand;
            trees[trees.Length - 1].transform.eulerAngles = transformRotationInHand;
        }
    }

}
