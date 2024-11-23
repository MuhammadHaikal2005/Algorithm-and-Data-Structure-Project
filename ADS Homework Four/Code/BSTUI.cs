using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationUI : MonoBehaviour
{
    public TMP_InputField findDroneInputField;
    public Button findDroneButton;
    public Button deleteDroneButton;

    public TMP_Text dronePosition;

    public DroneBTCommunication lessThanOrEqualBT;
    public DroneBTCommunication greaterThanBT;

    void Start(){
        findDroneButton.onClick.AddListener(onDroneButtonClicked);
        deleteDroneButton.onClick.AddListener(onDeleteButtonClicked);
    }

    // Function for finding drone button
    public void onDroneButtonClicked(){
        if (int.TryParse(findDroneInputField.text, out int ID)) {
            var result = lessThanOrEqualBT.FindDroneInBinarySearchTree(ID, drone => drone.ID);

            if (result.droneId != -1) {
                dronePosition.text = $"Drone ID: {result.droneId}\nPosition: {result.position}\nSearch Time: {result.searchTimeMilliseconds} ms";
            } 
            else {
                dronePosition.text = "Drone not found in network.";
            }
        } 
        else {
            dronePosition.text = "Invalid Drone ID.";
        }
    }

    // Function for deleting drone button 
    public void onDeleteButtonClicked(){
    if (int.TryParse(findDroneInputField.text, out int ID)) {
        var result = lessThanOrEqualBT.FindDroneInBinarySearchTree(ID, drone => drone.ID);
        if (result.droneId != -1) {
            // Delete the drone if it exists
            lessThanOrEqualBT.DeleteDroneFromBinarySearchTree(ID, drone => drone.ID);
            greaterThanBT.DeleteDroneFromBinarySearchTree(ID, drone => drone.ID);


            dronePosition.text = $"Deleted Drone ID: {result.droneId}\n" +
                                 $"Last Known Position: {result.position}\n" +
                                 $"Search Time: {result.searchTimeMilliseconds} ms";
        } else {
            dronePosition.text = $"Drone ID: {ID} not found in network.";
        }
    } else {
        dronePosition.text = "Invalid Drone ID.";
    }
}

}

