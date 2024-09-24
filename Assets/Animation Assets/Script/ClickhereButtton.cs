using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickhereButtton : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject scorePanel;

    private void Start()
    {
        if (inventoryPanel == null || scorePanel == null)
        {
            Debug.LogError("Panels are not assigned in the Inspector");
            return;
        }

        inventoryPanel.SetActive(true); // Hide the inventory panel by default
        scorePanel.SetActive(false);    // Show the score panel by default
    }

    public void OnClickHere()
    {
        inventoryPanel.SetActive(false);
        scorePanel.SetActive(true);  // Show the score panel when the button is clicked
    }
}

