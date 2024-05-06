using Backend.Models;
using Backend.Repositories;
using Backend.Services;

namespace Backend.Tests.Fakes
{
    public class FakeAccountRepository : IAccountRepository
    {
        private readonly IDataEncryptionService encryptionService;
        private BankAccount bankAccount;

        public FakeAccountRepository(BankAccount account, IDataEncryptionService encryptionService)
        {
            this.encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
            bankAccount = account ?? throw new ArgumentNullException(nameof(account));
        }

        public BankAccount BankAccount
        {
            get
            {
                // Simulate decryption by returning the original account details
                return bankAccount;
            }
            set
            {
                // Simulate encryption by setting the account details directly
                bankAccount = value;
            }
        }
    }
}
