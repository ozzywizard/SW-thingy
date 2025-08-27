using UnityEngine;
using static AIBehaviorEnum;
using System.Collections.Generic;

public class AIFight : MonoBehaviour
{
    public Transform player;
    public Transform playerShip;

    public float followDistance = 3f;
    public float resuppleDistance = 1.5f;

    public Queue<Transform> nodes = new Queue<Transform>();
    public AIState currentState = AIState.FollowingPlayer;

    private Transform currentTarget;
    private Transform nodeToDelete;


    private Rigidbody2D rb;

    public float maxVelocity = 3f;
    public float thrustAcceleration = 5f;
    public float thrustDamping = 2f;
    public float rotationAcceleration = 200f;
    public float rotationDamping = 2f;
    public float maxRotationSpeed = 3f;

    private float thrustVelocity = 0f;
    private float rotationVelocity = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        switch (currentState)
        {
            case AIState.FollowingPlayer:
                FollowPlayer();
                break;

            case AIState.GoingToNode:
                GotoNode();
                break;

            case AIState.Resupplying:
                Resupplying();
                break;
        }



    }

    void FollowPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > followDistance)
        {
            MoveTowards(player.position);
        }

        // If we have a node to go to, switch state
        if (nodes.Count > 0)
        {
            currentTarget = nodes.Dequeue();
            nodeToDelete = currentTarget;
            currentState = AIState.GoingToNode;
        }

    }

    void GotoNode()
    {
        if (currentTarget == null)
        {
            currentState = AIState.FollowingPlayer;
            return;
        }
        MoveTowards(currentTarget.position);

        if (Vector2.Distance(transform.position, currentTarget.position) < 0.5f)
        {
            //node reached, go resupple
            currentState = AIState.Resupplying;
        }
    }

    void Resupplying()
    {
        MoveTowards(playerShip.position);

        if (Vector2.Distance(transform.position, playerShip.position) < resuppleDistance)
        {
            Debug.Log("Resupplying....");

            //// Add your recharge logic here (e.g., refuel, reload)
            ///

            if (nodeToDelete != null)
            {
                Destroy(nodeToDelete.gameObject);
                nodeToDelete = null;
            }

            currentState = AIState.FollowingPlayer;
        }
    }

    void ApplyThrustInertia(Vector2 direction)
    {
        float input = Vector2.Dot(direction.normalized, transform.up); // how aligned we are
        thrustVelocity += input * thrustAcceleration * Time.deltaTime;

        // Damping when not accelerating
        if (Mathf.Approximately(input, 0f))
        {
            thrustVelocity = Mathf.Lerp(thrustVelocity, 0f, Time.deltaTime * thrustDamping);
        }

        //clamp velocity
        thrustVelocity = Mathf.Clamp(thrustVelocity, -maxVelocity, maxVelocity);

        rb.AddForce(transform.up * thrustVelocity);

    }

    void ApplyTurningInertia()
    {

    }

}
