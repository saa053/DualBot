using UnityEngine;

public class NPCMove : MonoBehaviour
{
    [SerializeField] Vector3 LocalTargetPos;
    [SerializeField] Quaternion targetRotation;
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;

    [SerializeField] Rigidbody body;
    [SerializeField] Animator animator;
    public Vector3 targetPos;
    public float stoppingDistance;

    bool manualMove = false;
    public bool trigger = false;
    public bool ready = false;

    bool inPos = false;
    bool inRot = false;

    void FixedUpdate()
    {
        if (!trigger)
        {
            if (RoomController.instance.currentRoom != null)
                targetPos = LocalTargetPos + RoomController.instance.currentRoom.GetRoomCenter();
            return;
        }

        manualMove = true;
        MoveNPC();

        if (inPos && !inRot)
        {
            RotateNPCToTarget();
        }
        
        if (inPos && inRot)
        {
            ready = true;
            trigger = false;
            manualMove = false;
        }
    }
    void MoveNPC()
    {
        Vector3 direction = targetPos - transform.position;

        if (direction.magnitude > stoppingDistance && !inPos)
        {
            body.velocity = direction.normalized * moveSpeed;
            animator.SetBool("isMoving", true);

            Quaternion toRotation = Quaternion.LookRotation(body.velocity, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed);
        }
        else
        {
            body.velocity = Vector3.zero;
            animator.SetBool("isMoving", false);
            inPos = true;
        }
    }

    void RotateNPCToTarget()
    {
        Quaternion toRotation = targetRotation;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed);

        if (transform.rotation == targetRotation)
            inRot = true;
    }

    public void SetNewTarget(Vector3 target, float stopMargin)
    {
        ready = false;
        inPos = false;
        inRot = false;
        targetPos = target;
        stoppingDistance = stopMargin;
    }

    public bool GetManualMove()
    {
        return manualMove;
    }
}
