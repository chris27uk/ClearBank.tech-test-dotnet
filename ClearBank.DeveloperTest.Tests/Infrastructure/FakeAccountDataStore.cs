using ClearBank.DeveloperTest.Features.MakePayment.Accounts;
using System.Collections.Generic;
using System.Linq;

namespace ClearBank.DeveloperTest.Tests.Infrastructure
{
    public class FakeAccountDataStore(Account[] accounts) : IAccountDataStore
    {
        private Dictionary<string, Account> accounts = accounts.ToDictionary(account => account.AccountNumber, account => account);
    
        public List<Account> Updates { get; } = new();
    
        public Account GetAccount(string accountNumber) => this.accounts.GetValueOrDefault(accountNumber);

        public void UpdateAccount(Account account) => this.Updates.Add(account);
    }
}
