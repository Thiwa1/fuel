using System;
using System.Collections.Generic;

namespace WpfApp1.Utilities
{
    public static class BankValidator
    {
        private static readonly Dictionary<string, int> _bankAccountLengths = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "BOC", 15 },
            { "Peoples", 15 },
            { "HNB", 12 },
            { "Commercial", 15 },
            { "Seylan", 12 },
            { "Sampath", 12 },
            { "NDB", 12 },
            { "Nations Trust", 12 },
            { "DFCC", 12 },
            { "Pan Asia", 12 },
            { "Union", 12 },
            { "Cargills", 12 },
            { "Amana", 12 },
            { "HSBC", 12 },
            { "Standard Chartered", 12 },
            { "Deutsche", 12 },
            { "Citibank", 10 }, // Example length, adjust as needed
            { "State Mortgage", 12 },
            { "Sanasa", 12 },
            { "RDB", 12 }
        };

        public static (bool IsValid, string Message) ValidateAccountNumber(string bankName, string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(bankName))
                return (true, ""); // Cannot validate without bank name

            if (string.IsNullOrWhiteSpace(accountNumber))
                return (false, "Account number is required.");

            // Try to find a partial match or exact match
            foreach (var kvp in _bankAccountLengths)
            {
                if (bankName.IndexOf(kvp.Key, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    int expectedLength = kvp.Value;
                    if (accountNumber.Length == expectedLength)
                    {
                        return (true, "Valid length.");
                    }
                    else if (accountNumber.Length < expectedLength)
                    {
                        return (false, $"Account number is too short. Expected {expectedLength} digits.");
                    }
                    else
                    {
                        return (false, $"Account number is too long. Expected {expectedLength} digits.");
                    }
                }
            }

            return (true, "Bank length not known."); // Pass if bank is not in our list
        }
    }
}
