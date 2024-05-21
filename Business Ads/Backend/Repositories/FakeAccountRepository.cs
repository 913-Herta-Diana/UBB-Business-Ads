using Backend.Models;
using Backend.Services;

namespace Backend.Repositories
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
                return bankAccount;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                bankAccount = value;
            }
        }
    }
}
