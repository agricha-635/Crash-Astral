using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovableBG : MonoBehaviour
{
    [SerializeField] private RawImage _img;  // Reference to the background image
    private float _y = 1.0f;  // Speed for vertical movement
    private float _x = 0f;    // Speed for horizontal movement, set to 0 for vertical only

    private bool isMoving = false; // Control whether the background is moving

    void Update()
    {
        if (isMoving)
        {
            // Scroll the image vertically
            _img.uvRect = new Rect(_img.uvRect.position + new Vector2(_x, _y) * Time.deltaTime, _img.uvRect.size);
        }
    }

    // Method to start the background movement
    public void StartMoving()
    {
        isMoving = true;
    }

    // Method to stop the background movement
    public void StopMoving()
    {
        isMoving = false;
    }
}