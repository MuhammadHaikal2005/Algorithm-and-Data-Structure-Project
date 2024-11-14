using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Drone : MonoBehaviour
{
    public int Temperature { get; private set; } // Encapsulated Temperature
    public int ID { get; private set; } // Unique ID for each drone
    public bool IsSurvivor { get; private set; } // New Survivor attribute

    private static int idCounter = 0; // Static counter for generating unique IDs
    private static bool survivorAssigned = false; // Track if Survivor has been assigned

    Flock agentFlock;
    public Flock AgentFlock { get { return agentFlock; } }

    Collider2D agentCollider;
    public Collider2D AgentCollider { get { return agentCollider; } }

    void Start()
    {
        agentCollider = GetComponent<Collider2D>();
    }

    // Initialize method
    public void Initialize(Flock flock)
    {
        agentFlock = flock;
        Temperature = (int)(Random.value * 100); // Assign temperature once
        ID = idCounter++; // Assign unique, increasing ID

        // Assign Survivor attribute randomly if not yet assigned
        if (!survivorAssigned && Random.value > 0.9f) // 10% chance to be Survivor
        {
            IsSurvivor = true;
            survivorAssigned = true;
            Debug.Log($"Drone {ID} is the SURVIVOR!");
        }
        else
        {
            IsSurvivor = false;
        }

    }

    public void Move(Vector2 velocity)
    {
        transform.up = velocity;
        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    // Method to handle incoming messages
    public void ReceiveMessage(string message)
    {
        Debug.Log($"Drone {ID} received message: {message}");
        // Handle the message logic here
    }

    // Self-destruct method
    public void SelfDestruct()
    {
        Debug.Log($"Drone {ID} is self-destructing.");
        gameObject.SetActive(false); // Hide the drone from the scene
    }
}

