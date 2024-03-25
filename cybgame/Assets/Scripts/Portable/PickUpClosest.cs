using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUpClosest : MonoBehaviour
{
    Transform player1;
    Transform player2;
    Portable closestPortableToPlayer1;
    Portable closestPortableToPlayer2;
    Portable[] portables;
    public static PickUpClosest instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        player1 = GameObject.FindWithTag("Player1").transform;
        player2 = GameObject.FindWithTag("Player2").transform;
    }
    void Update()
    {
        if (portables.Length == 0)
            portables = FindObjectsOfType<Portable>();

        closestPortableToPlayer1 = portables[0];
        closestPortableToPlayer1 = portables[0];

        foreach (Portable portable in portables)
        {
            if (Vector3.Distance(portable.transform.position, player1.position) < Vector3.Distance(closestPortableToPlayer1.transform.position, player1.position))
            {
                closestPortableToPlayer1 = portable;
            }

            if (Vector3.Distance(portable.transform.position, player2.position) < Vector3.Distance(closestPortableToPlayer2.transform.position, player2.position))
            {
                closestPortableToPlayer2 = portable;
            }
        }
    }

    public bool AmIClosestToPlayer(bool p1, Transform portable)
    {
        if (p1)
            return portable.GetComponent<Portable>() == closestPortableToPlayer1;
        else
            return portable.GetComponent<Portable>() == closestPortableToPlayer2;
    }   
}
