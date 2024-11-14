using System.Collections.Generic;
using UnityEngine;

public class DroneCommunication
{
    public Drone Drone { get; set; } // Reference to the Drone
    public DroneCommunication Next { get; set; } // Reference to the next node in the list

    public DroneCommunication(Drone drone)
    {
        Drone = drone;
        Next = null;
    }

    // Method to send a message to all drones in the partition
    public void SendMessageToPartition(string message)
    {
        DroneCommunication current = this; // Start from the head of the communication network
        while (current != null)
        {
            // Call a method on the Drone to handle the message
            current.Drone.ReceiveMessage(message);
            current = current.Next; // Move to the next node
        }
    }
}

