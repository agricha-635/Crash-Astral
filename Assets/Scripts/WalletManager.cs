// using UnityEngine;
// using TMPro;

// public class WalletManager : MonoBehaviour
// {
//     public static WalletManager Instance { get; private set; }

//     [SerializeField] private TMP_Text walletText;
//     private int walletAmount = 1000000; // Example initial wallet amount in rupees

//     private void Awake()
//     {
//         // Check if an instance already exists
//         if (Instance != null && Instance != this)
//         {
//             Destroy(gameObject); // Destroy this instance if it is a duplicate
//             return;
//         }

//         // Set this instance as the singleton
//         Instance = this;
//         DontDestroyOnLoad(gameObject); // Optional: Persist across scenes
//     }

//     private void Start()
//     {
//         UpdateWalletText();
//     }

//     public void ChangeWalletAmount(int amount)
//     {
//         walletAmount += amount;
//         UpdateWalletText();
//     }

//     public int GetWalletAmount()
//     {
//         return walletAmount;
//     }

//     private void UpdateWalletText()
//     {
//         walletText.text = FormatAmount(walletAmount);
//     }

//     private string FormatAmount(int amount)
//     {
//         if (amount >= 10000000) // 1 Crore
//         {
//             return $"{amount / 10000000}Cr";
//         }
//         else if (amount >= 100000) // 1 Lakh
//         {
//             return $"{amount / 100000}L";
//         }
//         else if (amount >= 1000) // 1 Thousand
//         {
//             return $"{amount / 1000}K";
//         }
//         else
//         {
//             return amount.ToString();
//         }
//     }
// }
