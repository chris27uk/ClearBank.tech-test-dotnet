namespace ClearBank.DeveloperTest.Features.MakePayment.Accounts;

public interface IAccountDataStore
{
    Account GetAccount(string accountNumber);
    
    void UpdateAccount(Account account);
}
