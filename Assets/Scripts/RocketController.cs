using UnityEngine;

public class RocketController : MonoBehaviour
{
    public GameObject Sprite;
    public RectTransform rocketTransform;  // Reference to the Rocket RectTransform
    private float speed = 200f;             // Speed of the Rocket
    private float rotationSpeed = 2f;       // Speed of rotation for smooth curve
    private Vector2 initialPosition = new Vector2(-324f, -336f);  // Initial position
    private Vector2 targetPosition = new Vector2(0f, 0f);         // Final position

    private bool isMovingX = true;
    private bool isMoving = false; // Control flag for movement

    private Quaternion initialRotation = Quaternion.Euler(0, 0, -90); // Initial rotation (horizontal)
    private Quaternion targetRotation = Quaternion.Euler(0, 0, 0);    // Target rotation (vertical)
    private bool isRotating = false;  // Flag to indicate if rocket is rotating
    private float rotationProgress = 0f; // Track rotation progress for smooth curve

    void Start()
    {
        Sprite.gameObject.SetActive(false);
        rocketTransform.gameObject.SetActive(false);
        ResetRocket(); // Set the initial state
    }

    void Update()
    {
        if (isMoving)
        {
            if (isMovingX)
            {
                // Move in the X direction
                rocketTransform.anchoredPosition = Vector2.MoveTowards(rocketTransform.anchoredPosition, new Vector2(0, initialPosition.y), speed * Time.deltaTime);

                // If rocket reaches the X = 0, start rotating and moving in Y direction
                if (rocketTransform.anchoredPosition.x == 0)
                {
                    isMovingX = false;
                    isRotating = true; // Start rotation after horizontal movement
                }
            }
            else if (isRotating)
            {
                // Smoothly rotate the rocket from 90 degrees to 0 degrees (curved transition)
                rotationProgress += rotationSpeed * Time.deltaTime; // Progress the rotation over time
                rocketTransform.rotation = Quaternion.Slerp(initialRotation, targetRotation, rotationProgress);

                // If the rotation is complete, stop rotating and move vertically
                if (rotationProgress >= 1f)
                {
                    isRotating = false;
                }
            }
            else
            {
                // Move in the Y direction after rotation is complete
                rocketTransform.anchoredPosition = Vector2.MoveTowards(rocketTransform.anchoredPosition, targetPosition, speed * Time.deltaTime);

                // Stop the rocket if it reaches the target position (0,0)
                if (rocketTransform.anchoredPosition == targetPosition)
                {
                    isMoving = false;
                }
            }
        }
    }

    // Method to start the rocket movement
    public void StartMoving()
    {
        Sprite.gameObject.SetActive(true);
        rocketTransform.gameObject.SetActive(true);
        isMoving = true;
    }

    // Method to stop the rocket movement
    public void StopMoving()
    {
        isMoving = false;
    }

    // Method to reset the rocket to its initial position and rotation
    public void ResetRocket()
    {
        rocketTransform.anchoredPosition = initialPosition;
        rocketTransform.rotation = initialRotation; // Set it to the horizontal rotation (-90 degrees)
        isMovingX = true;
        isRotating = false;
        rotationProgress = 0f; // Reset rotation progress
        isMoving = false;
    }
}
