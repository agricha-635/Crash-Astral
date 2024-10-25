using UnityEngine;

public class RocketController : MonoBehaviour
{
    public GameObject Sprite;             // Reference to the sprite GameObject
    public RectTransform rocketTransform; // Reference to the Rocket RectTransform
    [SerializeField] private GameObject boomImage;
    private Animator spriteAnimator;      // Animator reference for controlling animations
    private bool isAnimating = false;     // Track if the rocket is animating

    void Start()
    {
        // Get the Animator component from the Sprite GameObject
        spriteAnimator = Sprite.GetComponent<Animator>();

        // Initial setup
        Sprite.gameObject.SetActive(false);
        rocketTransform.gameObject.SetActive(false);
        boomImage.SetActive(false);
        spriteAnimator.enabled = false;
        ResetRocket(); // Set the initial state
    }

    // Method to start the rocket movement (animation)
    public void StartMoving()
    {
        Sprite.gameObject.SetActive(true);
        rocketTransform.gameObject.SetActive(true);
        spriteAnimator.enabled = true;   // Start the animation
        boomImage.SetActive(false);
        isAnimating = true;              // Rocket animation is now active
    }

    // Method to stop the rocket animation
    public void StopMoving()
    {
        rocketTransform.gameObject.SetActive(false);
        spriteAnimator.enabled = false; // Stop the animation
        boomImage.SetActive(true);
        isAnimating = false;            // Rocket animation has stopped
    }

    // Method to reset the rocket to its initial state
    public void ResetRocket()
    {
        Sprite.gameObject.SetActive(false);
    }

    void Update()
    {
        // Check if the rocket is animating
        if (isAnimating && spriteAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            // Animation has reached the end
            spriteAnimator.enabled = false;
        }
    }
}
