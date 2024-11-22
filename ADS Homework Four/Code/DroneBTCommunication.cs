using System;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneBTCommunication : MonoBehaviour {

    // Create node class
    public class BinarySearchTreeNode{
        public Drone drone;
        public BinarySearchTreeNode LeftNode, RightNode;

        public BinarySearchTreeNode(Drone drone){
            this.drone = drone;
            LeftNode = RightNode = null;
        }
    }

    
    private BinarySearchTreeNode root;
    private TMP_Text dronePosition;

    // Function for starting the binary search tree network
    public void StartSearchTreeNetwork(Drone[] drones, Func<Drone, int> keySelector){
        foreach (var drone in drones){
            root = InsertIntoTreeNetwork(root, drone, keySelector);
        }
    }

    // Function for inserting nodes into binary search tree network
    private BinarySearchTreeNode InsertIntoTreeNetwork(BinarySearchTreeNode node, Drone drone, Func<Drone, int> keySelector){
        if (node == null) return new BinarySearchTreeNode(drone);

        int key = keySelector(drone);
        int nodeKey = keySelector(node.drone);

        if (key < nodeKey)
            node.LeftNode = InsertIntoTreeNetwork(node.LeftNode, drone, keySelector);
        else if (key > nodeKey)
            node.RightNode = InsertIntoTreeNetwork(node.RightNode, drone, keySelector);

        return node;
    }

    // Function for finding drone in network
    public (int droneId, Vector3 position, float searchTimeMilliseconds) FindDroneInBinarySearchTree(int droneId, Func<Drone, int> keySelector){
        float totalDistance = 0f;
        var searchStatus = FindDrone(root, droneId, ref totalDistance, keySelector);

        if (searchStatus != null) {
            Debug.Log($"Drone {droneId} found at position {searchStatus.transform.position}");

            // Define the search speed (e.g., 2 meters per second)
            float searchSpeed = 2.0f; // Adjust this value as needed

            // Convert distance to time in seconds, then to milliseconds
            float searchTimeMilliseconds = (totalDistance / searchSpeed) * 1000f;

            return (droneId, searchStatus.transform.position, searchTimeMilliseconds);
            } 

            else {
                Debug.Log($"Drone {droneId} not found in network.");
                return (-1, Vector3.zero, -1f);
            }
    }


    // Function for finding drone
    private Drone FindDrone(BinarySearchTreeNode node, int droneId, ref float totalTime, Func<Drone, int> keySelector){
        if (node == null) return null;

        if (node.drone.ID == droneId) return node.drone;

        BinarySearchTreeNode nextNode = (droneId < keySelector(node.drone)) ? node.LeftNode : node.RightNode;
        if (nextNode != null)
            totalTime += Vector3.Distance(node.drone.transform.position, nextNode.drone.transform.position);

        return FindDrone(nextNode, droneId, ref totalTime, keySelector);
    }

    // Method for deleting a drone by its ID
    public void DeleteDroneFromBinarySearchTree(int droneId, Func<Drone, int> keySelector) {
        root = DeleteNode(root, droneId, keySelector);
    }


    private BinarySearchTreeNode DeleteNode(BinarySearchTreeNode node, int droneId, Func<Drone, int> keySelector) {
        if (node == null) return null;

        int nodeKey = keySelector(node.drone);


        if (droneId < nodeKey) {
            node.LeftNode = DeleteNode(node.LeftNode, droneId, keySelector);
        } else if (droneId > nodeKey) {
            node.RightNode = DeleteNode(node.RightNode, droneId, keySelector);
        } else {

            if (node.LeftNode == null) return node.RightNode;
            if (node.RightNode == null) return node.LeftNode;

            BinarySearchTreeNode successor = FindMinNode(node.RightNode);
            node.drone = successor.drone;

            node.RightNode = DeleteNode(node.RightNode, keySelector(successor.drone), keySelector);
        }

        return node;
    }


    private BinarySearchTreeNode FindMinNode(BinarySearchTreeNode node) {
        while (node.LeftNode != null) {
            node = node.LeftNode;
        }
        return node;
    }

    }

   
    




