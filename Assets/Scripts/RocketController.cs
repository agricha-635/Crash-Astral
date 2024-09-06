using UnityEngine;

public class RocketController : MonoBehaviour
{
    public RectTransform rocketTransform;  // Reference to the Rocket RectTransform
    private float speed = 200f;             // Speed of the Rocket

    private Vector2 initialPosition = new Vector2(-324f, -336f);  // Initial position
    private Vector2 targetPosition = new Vector2(0f, 0f);         // Final position

    private bool isMovingX = true;
    private bool isMoving = false; // Add a control flag for movement

    void Start()
    {
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

                // If rocket reaches the X = 0, switch to Y movement
                if (rocketTransform.anchoredPosition.x == 0)
                {
                    isMovingX = false;
                    rocketTransform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
            else
            {
                // Move in the Y direction
                rocketTransform.anchoredPosition = Vector2.MoveTowards(rocketTransform.anchoredPosition, targetPosition, speed * Time.deltaTime);

                // Stop the rocket if it reaches (0, 0)
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
        rocketTransform.rotation = Quaternion.Euler(0, 0, -90);
        isMovingX = true;
        isMoving = false;
    }
}
