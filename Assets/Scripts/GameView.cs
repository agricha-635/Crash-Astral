using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GameView : MonoBehaviour
{
    public static GameView Instance { get; private set; }

    [SerializeField]
    private Button playButton, acceptedButton, waitButton, cashOutButton;
    [SerializeField] private TMP_Text tenSecondTimerText, fourSecondTimerText, MultiplierText;
    [SerializeField] private TMP_InputField bettingAmount;
    [SerializeField] private GameObject WonCashPanel, StartingTimer, MultiplierPanel;
    [SerializeField] private Image CountDown, fillImage;
    [SerializeField] private Button increaseButton, decreaseButton;
    [SerializeField] private Button[] amountButtons; // Array for amount buttons
    [SerializeField] private TMP_Text walletText;

    private bool playButtonClicked = false;
    private int tenSecondTimer = 10;
    private int fourSecondTimer = 4;
    private int walletAmount = 1000000; // Example initial wallet amount in rupees

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
        if (int.TryParse(bettingAmount.text, out int betAmount))
        {
            if (betAmount <= walletAmount)
            {
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

        yield return new WaitForSeconds(2f);
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

        StartingTimer.gameObject.SetActive(false);

        // Wait for 5 seconds before restarting
        yield return new WaitForSeconds(2f);
        MultiplierPanel.gameObject.SetActive(true);
        StartCoroutine(RiseMultiplier());
    }

    private IEnumerator RiseMultiplier()
    {
        float multiplier = 1.00f;

        while (multiplier <= 3.00f)
        {
            MultiplierText.text = $"{multiplier:F2}x";
            multiplier += 0.01f;
            yield return new WaitForSeconds(0.1f); // Adjust the speed as needed
        }

        yield return new WaitForSeconds(3f); // Wait for 3 seconds after reaching 3.00x
        RestartGame();
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
        // Additional logic for cash out can be added here
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
}