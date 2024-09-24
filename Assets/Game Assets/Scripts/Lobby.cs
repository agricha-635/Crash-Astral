using UnityEngine;

public class Lobby : MonoBehaviour
{
    public static Lobby Instance { get; private set; }

    [SerializeField] public GameObject lobbyPanel; // Changed to public
    [SerializeField] public GameObject gameViewPanel; // Changed to public

    private void Awake()
    {
        // Check if an instance already exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy this instance if it is a duplicate
            return;
        }

        // Set this instance as the singleton
        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: Persist across scenes
    }

    private void Start()
    {
        lobbyPanel.SetActive(true);
        gameViewPanel.SetActive(false);
    }

    public void OnClickPlayButton()
    {
        lobbyPanel.SetActive(false);
        gameViewPanel.SetActive(true);
    }

    private void Update()
    {
        // Check for the Android back button press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
