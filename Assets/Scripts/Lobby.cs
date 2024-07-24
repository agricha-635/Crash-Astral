using UnityEngine;

public class Lobby : MonoBehaviour
{
    public static Lobby Instance { get; private set; }

    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject gameViewPanel;

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
}