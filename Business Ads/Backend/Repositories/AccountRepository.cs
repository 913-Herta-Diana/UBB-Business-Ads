// <copyright file="AccountRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Backend.Repositories
{
    using Backend.Models;
    using Backend.Services;

    public class AccountRepository : IAccountRepository
    {
        private readonly DataEncryptionService encryptionService = new ();
        private BankAccount bankAccount = new BankAccount();
        private string? emailKey;
        private string? nameKey;
        private string? surnameKey;
        private string? phoneNumberKey;
        private string? countyKey;
        private string? cityKey;
        private string? addressKey;
        private string? numberKey;
        private string? holderNameKey;
        private string? expirationDateKey;

        public AccountRepository(BankAccount account)
        {
            BankAccount = account ?? throw new ArgumentNullException(nameof(account));
        }

        public AccountRepository(BankAccount account, DataEncryptionService encryptionService)
        {
            this.encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
            BankAccount = account ?? throw new ArgumentNullException(nameof(account));
        }

        public BankAccount BankAccount
        {
            get
            {
                string? decryptedEmail = encryptionService.Decrypt(bankAccount.Email!, emailKey!);
                string? decryptedName = encryptionService.Decrypt(bankAccount.Name!, nameKey!);
                string? decryptedSurname = encryptionService.Decrypt(bankAccount.Surname!, surnameKey!);
                string? decryptedPhoneNumber = encryptionService.Decrypt(bankAccount.PhoneNumber!, phoneNumberKey!);
                string? decryptedCounty = encryptionService.Decrypt(bankAccount.County!, countyKey!);
                string? decryptedCity = encryptionService.Decrypt(bankAccount.City!, cityKey!);
                string? decryptedAddress = encryptionService.Decrypt(bankAccount.Address!, addressKey!);
                string? decryptedNumber = encryptionService.Decrypt(bankAccount.Number!, numberKey!);
                string? decryptedHolderName = encryptionService.Decrypt(bankAccount.HolderName!, holderNameKey!);
                string? decryptedExpiryDate = encryptionService.Decrypt(bankAccount.ExpiryDate!, expirationDateKey!);

                return new BankAccount
                {
                    Email = decryptedEmail,
                    Name = decryptedName,
                    Surname = decryptedSurname,
                    PhoneNumber = decryptedPhoneNumber,
                    County = decryptedCounty,
                    City = decryptedCity,
                    Address = decryptedAddress,
                    Number = decryptedNumber,
                    HolderName = decryptedHolderName,
                    ExpiryDate = decryptedExpiryDate,
                };
            }

            set
            {
                Dictionary<string, string> encryptedEmailKVPair = encryptionService.Encrypt(value.Email!);
                string encryptedEmail = encryptedEmailKVPair["data"];
                emailKey = encryptedEmailKVPair["key"];
                Dictionary<string, string> encryptedNameKVPair = encryptionService.Encrypt(value.Name!);
                string encryptedName = encryptedNameKVPair["data"];
                nameKey = encryptedNameKVPair["key"];
                Dictionary<string, string> encryptedSurnameKVPair = encryptionService.Encrypt(value.Surname!);
                string encryptedSurname = encryptedSurnameKVPair["data"];
                surnameKey = encryptedSurnameKVPair["key"];
                Dictionary<string, string> encryptedPhoneNumberKVPair = encryptionService.Encrypt(value.PhoneNumber!);
                string encryptedPhoneNumber = encryptedPhoneNumberKVPair["data"];
                phoneNumberKey = encryptedPhoneNumberKVPair["key"];
                Dictionary<string, string> encryptedCountyKVPair = encryptionService.Encrypt(value.County!);
                string encryptedCounty = encryptedCountyKVPair["data"];
                countyKey = encryptedCountyKVPair["key"];
                Dictionary<string, string> encryptedCityKVPair = encryptionService.Encrypt(value.City!);
                string encryptedCity = encryptedCityKVPair["data"];
                cityKey = encryptedCityKVPair["key"];
                Dictionary<string, string> encryptedAddressKVPair = encryptionService.Encrypt(value.Address!);
                string encryptedAddress = encryptedAddressKVPair["data"];
                addressKey = encryptedAddressKVPair["key"];
                Dictionary<string, string> encryptedNumberKVPair = encryptionService.Encrypt(value.Number!);
                string encryptedNumber = encryptedNumberKVPair["data"];
                numberKey = encryptedNumberKVPair["key"];
                Dictionary<string, string> encryptedHolderNameKVPair = encryptionService.Encrypt(value.HolderName!);
                string encryptedHolderName = encryptedHolderNameKVPair["data"];
                holderNameKey = encryptedHolderNameKVPair["key"];
                Dictionary<string, string> encryptedExpiryDateKVPair = encryptionService.Encrypt(value.ExpiryDate!);
                string encryptedExpiryDate = encryptedExpiryDateKVPair["data"];
                expirationDateKey = encryptedExpiryDateKVPair["key"];
                bankAccount = new BankAccount
                {
                    Email = encryptedEmail,
                    Name = encryptedName,
                    Surname = encryptedSurname,
                    PhoneNumber = encryptedPhoneNumber,
                    County = encryptedCounty,
                    City = encryptedCity,
                    Address = encryptedAddress,
                    Number = encryptedNumber,
                    HolderName = encryptedHolderName,
                    ExpiryDate = encryptedExpiryDate,
                };
            }
        }
    }
}
