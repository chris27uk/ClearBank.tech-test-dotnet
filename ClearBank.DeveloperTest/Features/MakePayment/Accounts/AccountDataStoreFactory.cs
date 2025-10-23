namespace ClearBank.DeveloperTest.Features.MakePayment.Accounts
{
    public class AccountDataStoreFactory(IAccountDataStore backupDataStore, IAccountDataStore accountDataStore)
    {
        public IAccountDataStore GetDataStore(string dataStoreType)
        {
            if (dataStoreType == "Backup")
            {
                return backupDataStore;
            }

            return accountDataStore;
        }
    }

}
