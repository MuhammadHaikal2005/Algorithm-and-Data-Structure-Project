using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flock : MonoBehaviour
{
    public Drone agentPrefab;
    Drone[] agents;
    public FlockBehavior behavior;

    [Range(10, 5000)] public int startingCount = 250;
    const float AgentDensity = 0.08f;

    [Range(1f, 100f)] public float driveFactor = 10f;
    [Range(1f, 100f)] public float maxSpeed = 5f;
    [Range(1f, 10f)] public float neighborRadius = 1.5f;
    [Range(0f, 1f)] public float avoidanceRadiusMultiplier = 0.5f;

    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }


    // Object Initialization for Linked List
    public LinkedListCommunication lessThanOrEqualLL;
    public LinkedListCommunication greaterThanLL;

    // Object Initialization for BT
    public DroneBTCommunication lessThanOrEqualBT;
    public DroneBTCommunication greaterThanBT;

    // Object Initialization for GN
    public DroneCommunicationNetwork lessThanOrEqualGN;
    public DroneCommunicationNetwork greaterThanGN;

    // Array for drone partitioning
    Drone[] lessThanOrEqual;
    Drone[] greaterThan;

    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;  

        agents = new Drone[startingCount];

        for (int i = 0; i < startingCount; i++)
        {
            Drone newAgent = Instantiate(
                agentPrefab,
                UnityEngine.Random.insideUnitCircle * startingCount * AgentDensity,
                Quaternion.Euler(Vector3.forward * UnityEngine.Random.Range(0f, 360f)),
                transform
            );
            newAgent.name = "Agent " + i;
            newAgent.Initialize(this);
            agents[i] = newAgent;
        }

        if (agents.Length > 0)
        {
            (lessThanOrEqual, greaterThan) = PartitionDrones(agents[1]);
        }

        // Manage which algorithm to use
        if (sceneName == "ADS Homework (Linked List)")
        {
            lessThanOrEqualLL.StartLinkedListNetwork(lessThanOrEqual);
            greaterThanLL.StartLinkedListNetwork(greaterThan);
        }
        else if (sceneName == "ADS Homework (Binary Search Tree)")
        {
            lessThanOrEqualBT.StartSearchTreeNetwork(lessThanOrEqual, agent => agent.ID);
            greaterThanBT.StartSearchTreeNetwork(greaterThan, agent => agent.ID);
        }
        else if (sceneName == "ADS Homework (Graph Network)")
        {
            lessThanOrEqualGN.StartGraphNetwork(lessThanOrEqual);
            greaterThanGN.StartGraphNetwork(greaterThan);
        }
 
            
        }

    public (Drone[] lessThanOrEqual, Drone[] greaterThan) PartitionDrones(Drone pivot)
    {
        float startTime = Time.time; // Start timer

        Drone[] lessThanOrEqual = new Drone[agents.Length];
        Drone[] greaterThan = new Drone[agents.Length];
        int lessIndex = 0;
        int greaterIndex = 0;

        foreach (Drone agent in agents)
        {
            if (agent.Temperature <= pivot.Temperature)
            {
                SetDroneColor(agent, Color.green);
                lessThanOrEqual[lessIndex++] = agent;
            }
            else
            {
                SetDroneColor(agent, Color.red);
                greaterThan[greaterIndex++] = agent;
            }
        }

        float endTime = Time.time; // End timer
        Debug.Log($"PartitionDrones ran in {(endTime - startTime) * 1000} milliseconds");

        // Resize arrays to fit actual counts
        System.Array.Resize(ref lessThanOrEqual, lessIndex);
        System.Array.Resize(ref greaterThan, greaterIndex);

        return (lessThanOrEqual, greaterThan);
    }

    void SetDroneColor(Drone drone, Color color)
    {
        SpriteRenderer spriteRenderer = drone.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
    }

    void Update()
    {
        foreach (Drone agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);
            Vector2 move = behavior.CalculateMove(agent, context, this) * driveFactor;

            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            agent.Move(move);
        }
    }

    List<Transform> GetNearbyObjects(Drone agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);

        foreach (Collider2D c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }
}

