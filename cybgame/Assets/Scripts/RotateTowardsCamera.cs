using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RotateTowardsCamera : MonoBehaviour
{
    void Update()
    {
        Vector3 directionToCamera = Camera.main.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(-directionToCamera, Vector3.up);
        transform.rotation = lookRotation;
    }
}
