using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftableObject : MonoBehaviour
{
    [SerializeField] float liftingMass;
    [SerializeField] float mass;

    ConfigurableJoint joint1;
    ConfigurableJoint joint2;
    Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        joint1 = GetComponent<ConfigurableJoint>();
        joint2 = GetComponents<ConfigurableJoint>()[1];
        body = GetComponent<Rigidbody>();
        body.mass = mass;
    }

    // Update is called once per frame
    void Update()
    {
        DetermineMass();
    }

    void DetermineMass()
    {
        if (joint1.connectedBody != null && joint2.connectedBody != null)
            body.mass = liftingMass;
        else
            body.mass = mass;
    }

    public void LiftObject(Rigidbody playerBody, ConfigurableJoint joint)
    {
        if (joint.connectedBody == null)
        {
            Vector3 anchorPos = transform.TransformPoint(joint.anchor);
            playerBody.transform.position = new Vector3(anchorPos.x, playerBody.transform.position.y, anchorPos.z);
            joint.connectedBody = playerBody;
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            joint.angularXMotion = ConfigurableJointMotion.Locked;
        }
    }

    public void DropObject(Rigidbody playerBody, ConfigurableJoint joint)
    {
        joint.connectedBody = null;
        joint.xMotion = ConfigurableJointMotion.Free;
        joint.yMotion = ConfigurableJointMotion.Free;
        joint.zMotion = ConfigurableJointMotion.Free;
        joint.angularXMotion = ConfigurableJointMotion.Free;
    }
}