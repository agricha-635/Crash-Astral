using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GameView : MonoBehaviour
{
    public static GameView Instance { get; private set; }

    [SerializeField]
    private Button playButton, acceptedButton, waitButton, cashOutButton, exitButton; // Add exitButton
    [SerializeField] private TMP_Text tenSecondTimerText, fourSecondTimerText, MultiplierText;
    [SerializeField] private TMP_InputField bettingAmount;
    [SerializeField] private GameObject WonCashPanel, StartingTimer, MultiplierPanel;
    [SerializeField] private Image CountDown, fillImage;
    [SerializeField] private Button increaseButton, decreaseButton;
    [SerializeField] private Button[] amountButtons; // Array for amount buttons
    [SerializeField] private TMP_Text walletText;
    [SerializeField] private TMP_Text CashOutAtMultiplier;
    [SerializeField] private TMP_Text WinningAmount;
    [SerializeField] private Transform historyPanel; // Parent panel for history entries
    [SerializeField] private GameObject historyEntryPrefab; // Prefab for history entries
    [SerializeField] private MovableBG movableBG; // Reference to the MovableBG component
    [SerializeField] private RocketController rocketController; // Reference to the RocketController
    [SerializeField] private GameObject boomImage;
    private bool playButtonClicked = false;
    private int tenSecondTimer = 10;
    private int fourSecondTimer = 4;
    private int walletAmount = 1000000; // Example initial wallet amount in rupees
    private float currentMultiplier = 1.00f; // Store the current multiplier value
    int betAmount = 0;

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
        playButton.onClick.AddListener(OnPlayButtonClick);
        cashOutButton.onClick.AddListener(OnCashOutButtonClick);
        increaseButton.onClick.AddListener(OnIncreaseButtonClick);
        decreaseButton.onClick.AddListener(OnDecreaseButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick); // Add listener for exitButton

        // Add listeners for the amount buttons
        foreach (Button button in amountButtons)
        {
            button.onClick.AddListener(() => OnAmountButtonClick(int.Parse(button.gameObject.name)));
        }

        // Initialize UI elements
        ResetUI();

        // Initialize wallet text
        UpdateWalletText();

        StartCoroutine(TenSecondTimer());
    }

    private void OnPlayButtonClick()
    {
        if (int.TryParse(bettingAmount.text, out betAmount))
        {
            if (betAmount <= walletAmount)
            {
                Debug.Log("Betting: " + betAmount);
                walletAmount -= betAmount;
                UpdateWalletText(); // Update wallet text after deduction

                playButtonClicked = true;
                // playButton.interactable = false;
                acceptedButton.gameObject.SetActive(true);
                playButton.gameObject.SetActive(false);
                bettingAmount.interactable = false;
                DisableRangeButtons();
            }
            else
            {
                Debug.Log("Insufficient funds in the wallet!");
            }
        }
    }

    private void UpdateWalletText()
    {
        walletText.text = FormatAmount(walletAmount);
    }

    private string FormatAmount(int amount)
    {
        if (amount >= 10000000) // 1 Crore
        {
            return $"{amount / 10000000}Cr";
        }
        else if (amount >= 100000) // 1 Lakh
        {
            return $"{amount / 100000}L";
        }
        else if (amount >= 1000) // 1 Thousand
        {
            return $"{amount / 1000}K";
        }
        else
        {
            return amount.ToString();
        }
    }

    private string FormatWinningAmount(float amount)
    {
        int intAmount = (int)amount;
        return FormatAmount(intAmount);
    }

    private IEnumerator TenSecondTimer()
    {
        while (tenSecondTimer >= 0)
        {
            tenSecondTimerText.text = tenSecondTimer.ToString();
            fillImage.fillAmount = tenSecondTimer / 10f;
            yield return new WaitForSeconds(1f);
            tenSecondTimer--;

            if (playButtonClicked)
            {
                bettingAmount.interactable = false;
                acceptedButton.gameObject.SetActive(true);
                playButton.gameObject.SetActive(false);
            }
        }

        tenSecondTimerText.text = "0";
        fillImage.fillAmount = 0f;

        if (!playButtonClicked)
        {
            bettingAmount.interactable = false;
            playButton.gameObject.SetActive(false);
            waitButton.gameObject.SetActive(true);
            DisableRangeButtons();
        }
        CountDown.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);
        StartingTimer.gameObject.SetActive(true);
        StartCoroutine(FourSecondTimer());
    }

    private IEnumerator FourSecondTimer()
    {
        while (fourSecondTimer >= 0)
        {
            fourSecondTimerText.text = fourSecondTimer.ToString();
            yield return new WaitForSeconds(1f);
            fourSecondTimer--;
        }

        fourSecondTimerText.text = "0";
        StartingTimer.gameObject.SetActive(false);

        // Wait for 5 seconds before restarting
        yield return new WaitForSeconds(1.0f);

        if (playButtonClicked)
        {
            acceptedButton.gameObject.SetActive(false);
            cashOutButton.gameObject.SetActive(true);
            waitButton.gameObject.SetActive(false);
        }
        else
        {
            waitButton.gameObject.SetActive(true);
        }

        MultiplierPanel.gameObject.SetActive(true);
        StartCoroutine(RiseMultiplier());
    }

    private IEnumerator RiseMultiplier()
    {
        float multiplier = 1.00f;
        float randomStop = Random.Range(1.00f, 10.00f); // Random stop point between 1.00x and 10.00x

        movableBG.StartMoving(); // Start the background movement
        rocketController.StartMoving(); // Start the rocket movement

        while (multiplier <= randomStop)
        {
            currentMultiplier = multiplier; // Update current multiplier
            MultiplierText.text = $"{multiplier:F2}x";
            multiplier += 0.01f;
            yield return new WaitForSeconds(0.1f); // Adjust the speed as needed
        }

        movableBG.StopMoving(); // Stop the background movement
        rocketController.StopMoving(); // Stop the rocket movement
        rocketController.rocketTransform.gameObject.SetActive(false);
        boomImage.SetActive(true); // Activate the boom image where the rocket crashes

        if (playButtonClicked)
        {
            cashOutButton.gameObject.SetActive(false);
            waitButton.gameObject.SetActive(true);
        }

        Debug.Log("Rocket Crash at: " + $"{randomStop:F2}x");

        // Add crash multiplier to history
        AddToHistory($"{randomStop:F2}x");

        yield return new WaitForSeconds(2f); // Wait for 3 seconds after reaching the random stop
        boomImage.SetActive(false); // Hide boom image after a delay
        rocketController.Sprite.gameObject.SetActive(false);
        RestartGame();
    }


    private void AddToHistory(string multiplier)
    {
        GameObject newEntry = Instantiate(historyEntryPrefab, historyPanel);
        TMP_Text textComponent = newEntry.GetComponentInChildren<TMP_Text>();
        textComponent.text = multiplier;
        newEntry.name = multiplier;
        // Remove oldest entry if more than 5
        if (historyPanel.childCount > 6)
        {
            Destroy(historyPanel.GetChild(0).gameObject);
        }
    }

    private void RestartGame()
    {
        // Reset the game state
        playButtonClicked = false;
        tenSecondTimer = 10;
        fourSecondTimer = 4;
        fillImage.fillAmount = 1f;
        bettingAmount.text = "1";
        UpdateWalletText(); // Ensure wallet is updated if needed

        // Reset UI elements to their initial state
        ResetUI();

        // Reset rocket position and rotation
        rocketController.ResetRocket();

        StartCoroutine(TenSecondTimer()); // Restart the timer
    }

    private void ResetUI()
    {
        playButton.gameObject.SetActive(true);
        acceptedButton.gameObject.SetActive(false);
        waitButton.gameObject.SetActive(false);
        cashOutButton.gameObject.SetActive(false);
        CountDown.gameObject.SetActive(true);
        StartingTimer.gameObject.SetActive(false);
        MultiplierPanel.gameObject.SetActive(false);
        WonCashPanel.gameObject.SetActive(false);
        playButton.interactable = true;
        bettingAmount.interactable = true;
        fillImage.fillAmount = 1f;
        increaseButton.interactable = true;
        decreaseButton.interactable = true;

        // Make amount buttons interactable only after 10-second timer or play button click
        foreach (Button button in amountButtons)
        {
            button.interactable = true; // Initially disable
        }
    }

    public void OnCashOutButtonClick()
    {
        // Set the cash-out multiplier text
        CashOutAtMultiplier.text = $"{currentMultiplier:F2}x";
        Debug.Log("Cash Out at Multiplier: " + $"{currentMultiplier:F2}x");

        // Calculate the winning amount
        float winningAmount = currentMultiplier * betAmount;

        // Update the wallet amount with winnings
        walletAmount = walletAmount + (int)winningAmount;

        // Check if the winning amount is less than 1000, then display actual value
        string displayWinningAmount;
        if (winningAmount < 1000)
        {
            displayWinningAmount = winningAmount.ToString("F2"); // Display actual amount with two decimal points
        }
        else
        {
            displayWinningAmount = FormatWinningAmount(winningAmount); // Display formatted amount
        }

        WinningAmount.text = displayWinningAmount; // Set the winning amount text
        walletText.text = FormatAmount(walletAmount); // Update wallet text
        Debug.Log("Winning amount: " + displayWinningAmount);
        Debug.Log("Wallet amount: " + walletAmount);

        // Activate/inactivate other buttons
        cashOutButton.gameObject.SetActive(false);
        WonCashPanel.gameObject.SetActive(true);
        waitButton.gameObject.SetActive(true);
    }


    private void OnIncreaseButtonClick()
    {
        // Increase the betting amount by 1
        if (int.TryParse(bettingAmount.text, out int currentValue))
        {
            bettingAmount.text = (currentValue + 1).ToString();
        }
    }

    private void OnDecreaseButtonClick()
    {
        // Decrease the betting amount by 1, ensuring it doesn't go below 1
        if (int.TryParse(bettingAmount.text, out int currentValue))
        {
            if (currentValue > 1)
            {
                bettingAmount.text = (currentValue - 1).ToString();
            }
        }
    }

    private void OnAmountButtonClick(int amount)
    {
        // Update the betting amount by the specified amount
        if (int.TryParse(bettingAmount.text, out int currentValue))
        {
            bettingAmount.text = (currentValue + amount).ToString();
        }
    }

    private void DisableRangeButtons()
    {
        foreach (Button button in amountButtons)
        {
            button.interactable = false; // Enable amount buttons
        }

        increaseButton.interactable = false;
        decreaseButton.interactable = false;
    }

    // Add this method to handle exit button click
    private void OnExitButtonClick()
    {
        // Switch back to the lobby view
        Lobby.Instance.lobbyPanel.SetActive(true);
        Lobby.Instance.gameViewPanel.SetActive(false);
    }
}