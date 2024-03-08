using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTransform : MonoBehaviour
{

    [SerializeField] Transform parentTransform;
    void Update()
    {
        /* transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one; */

        transform.position = parentTransform.position;
        transform.rotation = parentTransform.rotation;
        transform.localScale = parentTransform.localScale;
    }
}
