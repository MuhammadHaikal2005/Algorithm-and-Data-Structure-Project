using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DroneCommunicationNetwork : MonoBehaviour
{
    // Graph Node Class
    public class GraphNode
    {
        public Drone drone;
        public List<GraphNode> Neighbors;

        public GraphNode(Drone drone)
        {
            this.drone = drone;
            Neighbors = new List<GraphNode>();
        }
    }

    // Graph Network Class
    public class GraphNetwork
    {
        private List<GraphNode> nodes;

        public GraphNetwork()
        {
            nodes = new List<GraphNode>();
        }

        public GraphNode AddNode(Drone drone)
        {
            GraphNode newNode = new GraphNode(drone);
            nodes.Add(newNode);
            return newNode;
        }

        public void AddEdge(GraphNode node1, GraphNode node2)
        {
            if (node1 != null && node2 != null && !node1.Neighbors.Contains(node2))
            {
                node1.Neighbors.Add(node2);
                node2.Neighbors.Add(node1);
            }
        }

        public List<GraphNode> GetAllNodes()
        {
            return nodes;
        }

        public bool RemoveNode(GraphNode node)
        {
            if (node == null || !nodes.Contains(node)) return false;

            foreach (var neighbor in node.Neighbors)
            {
                neighbor.Neighbors.Remove(node);
            }

            nodes.Remove(node);
            return true;
        }
    }

    private GraphNetwork graph;

    // Function for starting the graph network
    public void StartGraphNetwork(Drone[] drones)
    {
        if (drones == null || drones.Length == 0)
        {
            Debug.LogError("No drones provided to initialize the graph network.");
            return;
        }

        graph = new GraphNetwork();

        // Map to track drone-to-node relationships
        Dictionary<Drone, GraphNode> droneNodeMap = new Dictionary<Drone, GraphNode>();
        foreach (var drone in drones)
        {
            var node = graph.AddNode(drone);
            droneNodeMap[drone] = node;
        }

        // Connect nodes based on proximity
        foreach (var node in droneNodeMap.Values)
        {
            foreach (var neighbor in droneNodeMap.Values)
            {
                if (node != neighbor && ShouldConnect(node.drone, neighbor.drone))
                {
                    graph.AddEdge(node, neighbor);
                }
            }
        }

        Debug.Log("Graph network initialized successfully.");
    }

    // Function to determine if two drones should be connected
    private bool ShouldConnect(Drone drone1, Drone drone2)
    {
        float maxDistance = 10.0f; // Adjust as needed
        return Vector3.Distance(drone1.transform.position, drone2.transform.position) <= maxDistance;
    }

    // Function for finding and displaying a drone in the graph network
    public (int droneId, Vector3 position, float searchTimeMilliseconds) FindAndDisplayDrone(int droneID)
    {
        if (graph == null)
        {
            Debug.LogError("Graph network has not been initialized.");
            return (-1, Vector3.zero, -1);
        }

        float startTime = Time.time;

        var node = graph.GetAllNodes().Find(n => n.drone.ID == droneID);

        float searchTimeMilliseconds = (Time.time - startTime) * 1000;

        if (node == null)
        {
            Debug.LogWarning($"Drone {droneID} not found in the graph network.");
            return (-1, Vector3.zero, searchTimeMilliseconds);
        }

        Debug.Log($"Drone {node.drone.ID} is at position: {node.drone.transform.position}");
        return (node.drone.ID, node.drone.transform.position, searchTimeMilliseconds);
    }

    // Function for finding and deleting a drone from the graph network
    public float FindAndDeleteDrone(int droneID)
    {
        if (graph == null)
        {
            Debug.LogError("Graph network has not been initialized.");
            return -1f;
        }

        float startTime = Time.time;

        var node = graph.GetAllNodes().Find(n => n.drone.ID == droneID);

        if (node == null)
        {
            Debug.LogWarning($"Drone {droneID} not found in the graph network. Cannot delete.");
            return (Time.time - startTime) * 1000;
        }

        bool removed = graph.RemoveNode(node);

        if (removed)
        {
            Debug.Log($"Drone {droneID} has been removed from the graph.");
        }
        else
        {
            Debug.LogError($"Failed to remove Drone {droneID} from the graph.");
        }

        return (Time.time - startTime) * 1000;
    }
}
