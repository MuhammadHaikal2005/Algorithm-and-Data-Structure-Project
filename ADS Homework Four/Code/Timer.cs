using UnityEngine;
using TMPro; // Include the TextMesh Pro namespace

public class TimeTracker : MonoBehaviour
{
    public TextMeshProUGUI timeText; // Reference to a TextMesh Pro UI component to display the time
    private float startTime;

    void Start()
    {
        // Record the start time
        startTime = Time.time;
    }

    public void DisplayElapsedTime(float elapsedTime)
    {
        // Update the TextMesh Pro component with the elapsed time
        timeText.text = "Time: " + elapsedTime.ToString("F2") + "s";
    }
}




