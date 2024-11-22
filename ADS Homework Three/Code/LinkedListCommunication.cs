using System;
using TMPro;
using UnityEngine;

public class LinkedListCommunication : MonoBehaviour
{
    // Node class for the linked list
    public class LinkedListNode
    {
        public Drone drone;
        public LinkedListNode NextNode;

        public LinkedListNode(Drone drone)
        {
            this.drone = drone;
            NextNode = null;
        }
    }

    private LinkedListNode head;

    // Function for starting the linked list network
    public void StartLinkedListNetwork(Drone[] drones)
    {
        foreach (var drone in drones)
        {
            InsertIntoLinkedList(drone);
        }

        // Debugging: Print the linked list after initialization
        PrintLinkedList();
    }

    // Function for inserting drones into the linked list
    private void InsertIntoLinkedList(Drone drone)
    {
        if (drone == null)
        {
            Debug.LogWarning("Attempted to insert a null drone into the linked list.");
            return;
        }

        if (head == null)
        {
            head = new LinkedListNode(drone);
            Debug.Log($"Drone {drone.ID} added as the head of the linked list.");
        }
        else
        {
            LinkedListNode current = head;
            while (current.NextNode != null)
            {
                current = current.NextNode;
            }
            current.NextNode = new LinkedListNode(drone);
            Debug.Log($"Drone {drone.ID} added to the linked list.");
        }
    }

    // Function for finding a drone in the linked list
    public (int droneId, Vector3 position, float searchTimeMilliseconds) FindDroneInLinkedList(int droneId)
    {
        if (head == null)
        {
            Debug.LogWarning("Linked list is empty. Cannot find any drone.");
            return (-1, Vector3.zero, -1f);
        }

        LinkedListNode current = head;
        float totalDistance = 0f; // Cumulative distance for search time calculation
        Vector3 previousPosition = head.drone.transform.position;

        while (current != null)
        {
            // Check for the drone ID
            if (current.drone.ID == droneId)
            {
                Debug.Log($"Drone {droneId} found at position {current.drone.transform.position}.");

                // Define the search speed (e.g., 2 meters per second)
                float searchSpeed = 2.0f; // Adjust as necessary

                // Convert distance to time in seconds, then to milliseconds
                float searchTimeMilliseconds = (totalDistance / searchSpeed) * 1000f;

                return (droneId, current.drone.transform.position, searchTimeMilliseconds);
            }

            // Update totalDistance
            totalDistance += Vector3.Distance(previousPosition, current.drone.transform.position);
            previousPosition = current.drone.transform.position;

            current = current.NextNode;
        }

        Debug.Log($"Drone {droneId} not found in the network.");
        return (-1, Vector3.zero, -1f);
    }

    // Function for deleting a drone from the linked list
    public void DeleteDroneFromLinkedList(int droneId)
    {
        if (head == null)
        {
            Debug.LogWarning("Linked list is empty. Cannot delete any drone.");
            return;
        }

        if (head.drone.ID == droneId)
        {
            Debug.Log($"Drone {droneId} deleted from the head of the linked list.");
            head = head.NextNode;
            return;
        }

        LinkedListNode current = head;

        while (current.NextNode != null && current.NextNode.drone.ID != droneId)
        {
            current = current.NextNode;
        }

        if (current.NextNode != null)
        {
            Debug.Log($"Drone {droneId} deleted from the linked list.");
            current.NextNode = current.NextNode.NextNode;
        }
        else
        {
            Debug.LogWarning($"Drone {droneId} not found in the linked list. No deletion performed.");
        }
    }

    // Function to print all drones in the linked list
    public void PrintLinkedList()
    {
        if (head == null)
        {
            Debug.Log("The linked list is empty.");
            return;
        }

        LinkedListNode current = head;
        Debug.Log("Printing linked list contents:");
        while (current != null)
        {
            Debug.Log($"Drone ID: {current.drone.ID}, Position: {current.drone.transform.position}");
            current = current.NextNode;
        }
    }
}
