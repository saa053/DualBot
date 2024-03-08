using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinLight : MonoBehaviour
{
    [SerializeField] float spinSpeed;

    void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
    }
}
