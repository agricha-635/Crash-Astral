using UnityEngine;
public class RocketController : MonoBehaviour
{
    public GameObject Sprite;
    public RectTransform rocketTransform;  // Reference to the Rocket RectTransform

    void Start()
    {
        Sprite.gameObject.SetActive(false);
        rocketTransform.gameObject.SetActive(false);
        ResetRocket(); // Set the initial state
    }
    // Method to start the rocket movement (animation)
    public void StartMoving()
    {
        Sprite.gameObject.SetActive(true);
        rocketTransform.gameObject.SetActive(true);
    }
    // Method to reset the rocket to its initial state
    public void ResetRocket()
    {
        rocketTransform.gameObject.SetActive(false); // Optionally deactivate it
    }
}
