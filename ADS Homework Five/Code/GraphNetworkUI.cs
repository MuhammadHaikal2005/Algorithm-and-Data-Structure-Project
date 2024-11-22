using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphNetworkUI : MonoBehaviour
{
    public TMP_InputField findDroneInputField;
    public Button findDroneButton;
    public Button deleteDroneButton;

    public TMP_Text dronePosition;

    public DroneCommunicationNetwork lessThanOrEqualGN;
    public DroneCommunicationNetwork greaterThanGN;

    void Start()
    {
        findDroneButton.onClick.AddListener(OnDroneButtonClicked);
        deleteDroneButton.onClick.AddListener(OnDeleteButtonClicked);
    }

    // Function for finding drone button
    public void OnDroneButtonClicked()
    {
        if (int.TryParse(findDroneInputField.text, out int ID))
        {
            string resultText = "";
            bool foundInNetwork = false;

            var result1 = lessThanOrEqualGN.FindAndDisplayDrone(ID);
            if (result1.droneId != -1)
            {
                resultText += $"Drone ID: {ID} found in LessThanOrEqual Network.\nPosition: {result1.position}\n";
                foundInNetwork = true;
            }

            var result2 = greaterThanGN.FindAndDisplayDrone(ID);
            if (result2.droneId != -1)
            {
                resultText += $"Drone ID: {ID} found in GreaterThan Network.\nPosition: {result2.position}\n";
                foundInNetwork = true;
            }

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

    // Function for deleting drone button
    public void OnDeleteButtonClicked()
    {
        if (int.TryParse(findDroneInputField.text, out int ID))
        {
            float timeOne = lessThanOrEqualGN.FindAndDeleteDrone(ID);
            float timeTwo = greaterThanGN.FindAndDeleteDrone(ID);

            dronePosition.text = $"Drone ID: {ID} has been deleted from both networks.\n" +
                                 $"Time taken for LessThanOrEqual network: {timeOne:F2} ms\n" +
                                 $"Time taken for GreaterThan network: {timeTwo:F2} ms";
        }
        else
        {
            dronePosition.text = "Invalid Drone ID.";
        }
    }
}
