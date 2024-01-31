using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class LiftableObject : MonoBehaviour
{
    [SerializeField] float liftHeight;
    [SerializeField] float liftingMass;
    [SerializeField] float mass;

    bool player1InRange;
    bool player2InRange;

    ConfigurableJoint joint1;
    ConfigurableJoint joint2;
    Rigidbody body;

    PlayerInputManager player1Input;
    PlayerInputManager player2Input;
    Rigidbody player1Body;
    Rigidbody player2Body;

    void Start()
    {
        joint1 = GetComponent<ConfigurableJoint>();
        joint2 = GetComponents<ConfigurableJoint>()[1];
        body = GetComponent<Rigidbody>();
        body.mass = mass;

        player1Input = GameObject.FindWithTag("Player1").GetComponent<PlayerInputManager>();
        player2Input = GameObject.FindWithTag("Player2").GetComponent<PlayerInputManager>();

        player1Body = GameObject.FindWithTag("Player1").GetComponent<Rigidbody>();
        player2Body = GameObject.FindWithTag("Player2").GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleLifting(player1InRange, player1Input, player1Body);
        HandleLifting(player2InRange, player2Input, player2Body);

        DetermineMass();
    }

    void HandleLifting(bool playerInRange, PlayerInputManager inputManager, Rigidbody playerBody)
    {
        if (!inputManager.GetInteract())
            return;

        if (inputManager.IsLifting())
        {
            if (joint1.connectedBody == playerBody)
            {
                DropObject(joint1);
                inputManager.SetIsLifting(false);
            }
            else if (joint2.connectedBody == playerBody)
            {
                DropObject(joint2);
                inputManager.SetIsLifting(false);
            }
            
            return;
        }

        if (!playerInRange)
            return;
            
        if (joint1.connectedBody == null)
        {
            LiftObject(playerBody, joint1);
            inputManager.SetIsLifting(true);
        }
        else if (joint2.connectedBody == null)
        {
            LiftObject(playerBody, joint2);
            inputManager.SetIsLifting(true);
        }
        
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
            joint.connectedAnchor = new Vector3(0, liftHeight, 0);
            joint.connectedBody = playerBody;
            Vector3 newAnchorPos = transform.InverseTransformPoint(playerBody.transform.position);
            joint.anchor = newAnchorPos;

            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            joint.angularXMotion = ConfigurableJointMotion.Locked;
        }
    }

    public void DropObject(ConfigurableJoint joint)
    {
        joint.connectedBody = null;
        joint.xMotion = ConfigurableJointMotion.Free;
        joint.yMotion = ConfigurableJointMotion.Free;
        joint.zMotion = ConfigurableJointMotion.Free;
        joint.angularXMotion = ConfigurableJointMotion.Free;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player1")
            player1InRange = true;
        else if (other.gameObject.tag == "Player2")
            player2InRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player1")
            player1InRange = false;
        else if (other.gameObject.tag == "Player2")
            player2InRange = false;
    }
}