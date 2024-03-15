using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portable : MonoBehaviour
{
    [Header ("Portable FX")]
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
    bool player1IsCarry;
    bool player2IsCarry;
    Transform player1;
    Transform player2;
    PlayerInputManager player1Input;
    PlayerInputManager player2Input;
    Animator player1Animator;
    Animator player2Animator;
    
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

        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        DisplayInfoWhenPlayerClose();

        if (player1IsCarry && player1Input.GetInteract() && transform.parent == player1)
        {
            player1IsCarry = false;
            Drop();
            player1Animator.SetBool("isCarry", false);
            ResetPlayerHitbox(player1.GetComponent<CapsuleCollider>());
        }
        
        if (player2IsCarry && player2Input.GetInteract() && transform.parent == player2)
        {
            player2IsCarry = false;
            Drop();
            player2Animator.SetBool("isCarry", false);
            ResetPlayerHitbox(player2.GetComponent<CapsuleCollider>());
        }
            
        if (trigger.Player1Trigger() && !player1IsCarry && !locked && player1Input.GetComponentInChildren<Portable>() == null)
        {
            player1IsCarry = true;
            PickUp(player1);
            player1Animator.SetBool("isCarry", true);
            IncreasePlayerHitbox(player1.GetComponent<CapsuleCollider>());
        }
        
        if (trigger.Player2Trigger() && !player2IsCarry && !locked && player2Input.GetComponentInChildren<Portable>() == null)
        {
            player2IsCarry = true;
            PickUp(player2);
            player2Animator.SetBool("isCarry", true);
            IncreasePlayerHitbox(player2.GetComponent<CapsuleCollider>());
        }
    }

    void PickUp(Transform parent)
    {
        transform.rotation = parent.rotation;
        transform.position = parent.transform.position;
        
        transform.position += parent.forward * distanceFromPlayer;
        transform.position += new Vector3(0, height, 0);

        body.useGravity = false;
        body.isKinematic = true;

        boxCollider.isTrigger = true;

        transform.parent = parent;
    }

    void Drop()
    {
        body.useGravity = true;
        body.isKinematic = false;

        boxCollider.isTrigger = false;

        transform.parent = RoomController.instance.currentRoom.transform;
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
        Drop();
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