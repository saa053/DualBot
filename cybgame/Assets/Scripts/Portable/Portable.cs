using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Portable : MonoBehaviour
{
    [Header ("Portable FX")]
    [SerializeField] Color outlineColor;
    [SerializeField] float implodeMaxSize;
    [SerializeField] float implodeMinSize;
    [SerializeField] float implodeSpeed;
    [SerializeField] bool destroy;

    [Header ("Portable Info")]
    [SerializeField] GameObject canvas;
    bool isSafe;
    string explanation;
    int result = -1;

    [Header ("Player Hitbox")]
    [SerializeField] float carryHeight = 1.482718f;
    [SerializeField] float carryRadius = 0.4676035f;
    [SerializeField] Vector3 carryCenter = new Vector3(6.608116e-09f, 0.6999999f, 0.2174886f);
    [SerializeField] float originalHeight = 1.482718f;
    [SerializeField] float originalRadius = 0.4676035f;
    [SerializeField] Vector3 originalCenter = new Vector3(6.608116e-09f, 0.6999999f, 0.2174886f);

    [Header ("Carry settings")]
    [SerializeField] float distanceFromPlayer;
    [SerializeField] float height;

    bool locked = false;
    
    Rigidbody body;
    Trigger trigger;
    BoxCollider boxCollider;

    [Header("Players")]
    bool isCarried = false;
    Transform player1;
    Transform player2;
    PlayerInputManager player1Input;
    PlayerInputManager player2Input;
    Animator player1Animator;
    Animator player2Animator;

    bool player1Info = false;
    bool player2Info = false;

    Outline outline;
    
    void Start()
    {
        body = GetComponent<Rigidbody>();  
        trigger = transform.Find("Trigger").GetComponent<Trigger>();
        boxCollider = GetComponent<BoxCollider>();
        player1 = GameObject.FindWithTag("Player1").transform;
        player2 = GameObject.FindWithTag("Player2").transform;
        player1Input = GameObject.FindWithTag("Player1").GetComponent<PlayerInputManager>();
        player2Input = GameObject.FindWithTag("Player2").GetComponent<PlayerInputManager>();
        player1Animator = player1.GetComponentInChildren<Animator>();
        player2Animator = player2.GetComponentInChildren<Animator>();
        outline = GetComponentInChildren<Outline>();

        canvas.SetActive(false);
    }

    void Update()
    {
        bool p1Trigger = trigger.Player1Trigger();
        bool p2Trigger = trigger.Player2Trigger();

        DisplayOutlineAndInfo();

        CheckPlayer(true, trigger.Player1Close(), p1Trigger, player1, player2, player1Animator, player2Animator, player1Input, player2Input);
        CheckPlayer(false, trigger.Player2Close(), p2Trigger, player2, player1, player2Animator, player1Animator, player2Input, player1Input);
    }

    void CheckPlayer(bool p1, bool playerClose, bool interact, Transform playerTransform, Transform otherPlayerTransform, Animator animator, Animator otherAnimator, PlayerInputManager input, PlayerInputManager otherInput)
    {
        if (playerClose && interact)
        {
            if (input.GetCarry() && transform.parent == playerTransform)
            {
                Drop(animator, input);
            }
            else if (!locked && !input.GetCarry())
            {
                Portable[] allPortables = FindObjectsOfType<Portable>();
                foreach (Portable item in allPortables)
                {
                    if (Vector3.Distance(transform.position, playerTransform.position) > Vector3.Distance(item.transform.position, playerTransform.position))
                    {
                        input.ResetInteract(interact);
                        return;
                    }

                }

                if (transform.parent == otherPlayerTransform)
                {
                    Drop(otherAnimator, otherInput);
                }

                PickUp(playerTransform, animator, input);
            }
            
            if (input.GetCarry() && transform.parent != playerTransform)
            {
                input.ResetInteract(interact);
            }
        }
    }

    void PickUp(Transform parent, Animator animator, PlayerInputManager input)
    {
        transform.rotation = parent.rotation;
        transform.position = parent.transform.position;
        
        transform.position += parent.forward * distanceFromPlayer;
        transform.position += new Vector3(0, height, 0);

        body.useGravity = false;
        body.isKinematic = true;

        boxCollider.isTrigger = true;

        transform.parent = parent;

        isCarried = true;
        animator.SetBool("isCarry", true);
        input.SetCarry(true);
        IncreasePlayerHitbox(input.transform.GetComponent<CapsuleCollider>());
    }

    void Drop(Animator animator, PlayerInputManager input)
    {
        body.useGravity = true;
        body.isKinematic = false;

        boxCollider.isTrigger = false;

        transform.parent = RoomController.instance.currentRoom.transform;

        isCarried = false;
        animator.SetBool("isCarry", false);
        input.SetCarry(false);
        ResetPlayerHitbox(input.transform.GetComponent<CapsuleCollider>());
    }

    void IncreasePlayerHitbox(CapsuleCollider collider)
    {
        collider.height = carryHeight;
        collider.radius = carryRadius;
        collider.center = carryCenter;
    }

    void ResetPlayerHitbox(CapsuleCollider collider)
    {
        collider.height = originalHeight;
        collider.radius = originalRadius;
        collider.center = originalCenter;
    }

    void DisplayOutlineAndInfo()
    {
        if (locked)
            return;

        if ((trigger.Player1Close() && !player1Input.GetCarry()) || (trigger.Player1Close() && transform.parent == player1))
        {
            Portable[] allPortables = FindObjectsOfType<Portable>();
            foreach (Portable item in allPortables)
            {
                if (Vector3.Distance(transform.position, player1.position) > Vector3.Distance(item.transform.position, player1.position))
                {
                    player1Info = false;

                    canvas.SetActive(false);

                    if (outline.OutlineColor == outlineColor)
                        outline.OutlineWidth = 0;
                    return;
                }

            }

            player1Info = true;
            canvas.SetActive(true);

            if (!isCarried)
            {
                outline.OutlineWidth = 10;
                outline.OutlineColor = outlineColor;
            }
            else
            {
                if (outline.OutlineColor == outlineColor)
                    outline.OutlineWidth = 0;
            }

            return;
        }
        else if ((trigger.Player2Close() && !player2Input.GetCarry()) || (trigger.Player2Close() && transform.parent == player2))
        {
            Portable[] allPortables = FindObjectsOfType<Portable>();
            foreach (Portable item in allPortables)
            {
                if (Vector3.Distance(transform.position, player2.position) > Vector3.Distance(item.transform.position, player2.position))
                {
                    player2Info = false;

                    canvas.SetActive(false);

                    if (outline.OutlineColor == outlineColor)
                        outline.OutlineWidth = 0;
                    return;
                }

            }

            player2Info = true;
            canvas.SetActive(true);

            if (!isCarried)
            {
                outline.OutlineWidth = 10;
                outline.OutlineColor = outlineColor;
            }
            else
            {
                if (outline.OutlineColor == outlineColor)
                    outline.OutlineWidth = 0;
            }

            return;
        }



        canvas.SetActive(false);

        if (outline.OutlineColor == outlineColor)
            outline.OutlineWidth = 0;
    }

    void DrawOutline()
    {
        if (locked)
            return;
        if (trigger.Player1Close() || trigger.Player2Close())
        {
            if (!isCarried)
            {
                outline.OutlineWidth = 10;
                outline.OutlineColor = outlineColor;
            }
            else
            {
                if (outline.OutlineColor == outlineColor)
                    outline.OutlineWidth = 0;
            }
        } 
        else
        {
            if (outline.OutlineColor == outlineColor)
                outline.OutlineWidth = 0;
        }
    }

    void DisplayInfoWhenPlayerClose()
    {
        if (locked)
            return;

        if (trigger.Player1Close() || trigger.Player2Close())
            canvas.SetActive(true);
        else
            canvas.SetActive(false);
    }

    public void SetSafe(bool value)
    {
        isSafe = value;
    }

    public bool GetSafe()
    {
        return isSafe;
    }

    public void SetExplanation(string value)
    {
        explanation = value;
    }

    public string GetExplanation()
    {
        return explanation;
    }

    public void ToggleCanvas(bool value)
    {
        canvas.SetActive(value);
    }

    public void Lock()
    {
        if (isCarried)
        {
            Animator animator = transform.parent.GetComponentInChildren<Animator>();
            PlayerInputManager input = transform.parent.GetComponent<PlayerInputManager>();
            Drop(animator, input);
        }
        
        body.mass = 200;
        locked = true;
        canvas.SetActive(false);
    }

    public IEnumerator Implode(ParticleSystem fx)
    {
        /* while (transform.localScale.x < implodeMaxSize)
        {
            transform.localScale += new Vector3(implodeSpeed, implodeSpeed, implodeSpeed);
            yield return new WaitForEndOfFrame();
        } */

        fx.Play();
        while (transform.localScale.x > implodeMinSize)
        {
            transform.localScale -= new Vector3(implodeSpeed, implodeSpeed, implodeSpeed);
            yield return new WaitForEndOfFrame();
        }
        Destroy(fx.gameObject);


        Destroy(this.gameObject);
    }

    public void SetResult(int res)
    {
        result = res;
    }

    public int GetResult()
    {
        return result;
    }
}
