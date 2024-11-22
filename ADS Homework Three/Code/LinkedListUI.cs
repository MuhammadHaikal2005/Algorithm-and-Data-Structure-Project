using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LinkedListUI : MonoBehaviour
{
    public TMP_InputField findDroneInputField;
    public Button findDroneButton;
    public Button deleteDroneButton;

    public TMP_Text dronePosition;

    public LinkedListCommunication lessThanOrEqualLL;
    public LinkedListCommunication greaterThanLL;

    void Start()
    {
        findDroneButton.onClick.AddListener(OnDroneButtonClicked);
        deleteDroneButton.onClick.AddListener(OnDeleteButtonClicked);
    }

    // Function for the "Find Drone" button
    public void OnDroneButtonClicked()
    {
        if (int.TryParse(findDroneInputField.text, out int ID))
        {
            string resultText = "";
            bool foundInNetwork = false;

            // Search in the LessThanOrEqual network
            var result1 = lessThanOrEqualLL.FindDroneInLinkedList(ID);
            if (result1.droneId != -1)
            {
                Debug.Log($"Drone ID {ID} found in LessThanOrEqual network.");
                resultText += $"Drone ID: {ID} found in LessThanOrEqual Network.\n" +
                            $"Position: {result1.position}\n" +
                            $"Search Time: {result1.searchTimeMilliseconds} ms\n";
                foundInNetwork = true;
            }
            else
            {
                Debug.Log($"Drone ID {ID} not found in LessThanOrEqual network.");
            }

            // Search in the GreaterThan network
            var result2 = greaterThanLL.FindDroneInLinkedList(ID);
            if (result2.droneId != -1)
            {
                Debug.Log($"Drone ID {ID} found in GreaterThan network.");
                resultText += $"Drone ID: {ID} found in GreaterThan Network.\n" +
                            $"Position: {result2.position}\n" +
                            $"Search Time: {result2.searchTimeMilliseconds} ms\n";
                foundInNetwork = true;
            }
            else
            {
                Debug.Log($"Drone ID {ID} not found in GreaterThan network.");
            }

            // If not found in either network
            if (!foundInNetwork)
            {
                resultText = "Drone not found in either network.";
            }

            dronePosition.text = resultText;
        }
        else
        {
            dronePosition.text = "Invalid Drone ID.";
        }
    }


    // Function for the "Delete Drone" button
    public void OnDeleteButtonClicked()
    {
        if (int.TryParse(findDroneInputField.text, out int ID))
        {
            bool deletedFromAnyNetwork = false;

            // Delete from LessThanOrEqual network
            lessThanOrEqualLL.DeleteDroneFromLinkedList(ID);
            deletedFromAnyNetwork = true;

            // Delete from GreaterThan network
            greaterThanLL.DeleteDroneFromLinkedList(ID);
            deletedFromAnyNetwork = true;

            if (deletedFromAnyNetwork)
            {
                dronePosition.text = $"Drone ID: {ID} has been deleted from both networks.";
            }
            else
            {
                dronePosition.text = "Drone not found in either network, so no deletion occurred.";
            }
        }
        else
        {
            dronePosition.text = "Invalid Drone ID.";
        }
    }
}
