using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftManager : MonoBehaviour
{
    Transform player1;
    Transform player2;
    ConfigurableJoint[] joints;

    public static LiftManager instance;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        joints = FindObjectsOfType<ConfigurableJoint>();
        player1 = GameObject.FindWithTag("Player1").GetComponent<PlayerMovement>().transform;
        player2 = GameObject.FindWithTag("Player2").GetComponent<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ConfigurableJoint GetClosestJoint(Transform player)
    {
        if (joints[0] == null)
        {
            Debug.Log("No joints in scene!");
            return null;
        }

        ConfigurableJoint closestJoint = joints[0];
        float distance;
        float closestDistance = Vector3.Distance(player.position, joints[0].transform.TransformPoint(joints[0].anchor));

        foreach (ConfigurableJoint joint in joints)
        {
            distance = Vector3.Distance(player.position, joints[0].transform.transform.TransformPoint(joint.anchor));

            if (distance < closestDistance && joint.connectedBody == null)
            {
                closestDistance = distance;
                closestJoint = joint;
            }
        }

        return closestJoint;
    }
}
