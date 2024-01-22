using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerLift : MonoBehaviour
{
    [SerializeField] float pickupDistance;
    [SerializeField] bool gizmo;
    Rigidbody body;

    ConfigurableJoint currentJoint;

    ConfigurableJoint targetJoint;

    PlayerInputManager inputManager;
    void Start()
    {
        body = GetComponent<Rigidbody>();
        inputManager = GetComponent<PlayerInputManager>();
    }

    void Update()
    {
        targetJoint = LiftManager.instance.GetClosestJoint(transform);
        if (targetJoint == null)
            return;

        if (inputManager.GetInteract())
            Lift();
    }

    void Lift()
    {
        LiftableObject jointScript = targetJoint.GetComponent<LiftableObject>();
        
        if (currentJoint != null)
        {
            jointScript.DropObject(body, currentJoint);
            currentJoint = null;
            return;
        }

        Vector3 jointPos = targetJoint.transform.TransformPoint(targetJoint.anchor);
        if (Vector3.Distance(transform.position, jointPos) < pickupDistance)
        {
            jointScript.LiftObject(body, targetJoint);
            currentJoint = targetJoint;
        }
        else 
        {
            Debug.Log("Too far away to lift up!");
        }
    }

    /* private void OnDrawGizmos()
    {
        if (!gizmo)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupDistance);

        if (targetJoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(jointPos, 0.4f);
        }
    } */
}
