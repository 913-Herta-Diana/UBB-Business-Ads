// <copyright file="AccountRepositoryTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Backend.Tests
{
    using System;
    using Backend.Models;
    using Backend.Repositories;
    using Backend.Services;
    using Backend.Tests.Fakes;
    using Moq;
    using Xunit;

    public class AccountRepositoryTests
    {
        [Fact]
        public void Constructor_NullAccount_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new FakeAccountRepository(null!, Mock.Of<IDataEncryptionService>()));
        }

        [Fact]
        public void GetterAndSetter_PropertiesEncryptedAndDecryptedSuccessfully()
        {
            // Arrange
            var mockDataEncryptionService = new Mock<IDataEncryptionService>();
            mockDataEncryptionService
                .Setup(x => x.Decrypt(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string data, string key) => data.Replace("Encrypted", " "));

            var account = new BankAccount
            {
                Email = "example@example.com",
                Name = "John Doe",
                Surname = "Doe",
            };

            var repository = new FakeAccountRepository(account, mockDataEncryptionService.Object);

            // Act
            var retrievedAccount = repository.BankAccount;

            // Assert
            Assert.Equal(account.Email, retrievedAccount.Email);
            Assert.Equal(account.Name, retrievedAccount.Name);
            Assert.Equal(account.Surname, retrievedAccount.Surname);
        }

        [Fact]
        public void Setter_NullAccount_ThrowsArgumentNullException()
        {
            // Arrange
            var mockDataEncryptionService = new Mock<IDataEncryptionService>();
            var repository = new FakeAccountRepository(new BankAccount(), mockDataEncryptionService.Object);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => repository.BankAccount = null!);
        }

        [Fact]
        public void Setter_PropertiesEncryptedSuccessfully()
        {
            // Arrange
            var mockDataEncryptionService = new Mock<IDataEncryptionService>();
            mockDataEncryptionService.Setup(x => x.Encrypt(It.IsAny<string>())).Returns((string data) => new Dictionary<string, string> { { "data", data + "Encrypted" }, { "key", "Key" } });

            var repository = new FakeAccountRepository(new BankAccount(), mockDataEncryptionService.Object);

            var account = new BankAccount
            {
                Email = "example@example.com",
                Name = "John Doe",
                Surname = "Doe",
            };

            // Act
            repository.BankAccount = account;
            var retrievedAccount = repository.BankAccount;

            // Assert
            Assert.Equal(account.Email, retrievedAccount.Email);
            Assert.Equal(account.Name, retrievedAccount.Name);
            Assert.Equal(account.Surname, retrievedAccount.Surname);
        }

        [Fact]
        public void Integration_TestEncryptionAndDecryption()
        {
            // Arrange
            var dataEncryptionService = new DataEncryptionService();
            var accountRepository = new FakeAccountRepository(new BankAccount(), dataEncryptionService);

            var originalData = "example@example.com";

            // Act
            var encryptedData = accountRepository.BankAccount.Email; // Simulated encryption
            var decryptedData = dataEncryptionService.Decrypt(encryptedData!, "Key");

            // Assert
            Assert.NotEqual(originalData, encryptedData);
            Assert.Equal(originalData, decryptedData);
        }
    }
}
