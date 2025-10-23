using ClearBank.DeveloperTest.Features.MakePayment.Accounts;
using ClearBank.DeveloperTest.Infrastructure.Accounts;
using ClearBank.DeveloperTest.Types;
using System;
using System.Configuration;

namespace ClearBank.DeveloperTest.Features.MakePayment
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStore primaryAccountDataStore;
        private readonly IAccountDataStore backupAccountDataStore;
        private readonly Func<string> getDataStoreType;

        public PaymentService(IAccountDataStore primaryAccountDataStore,
            IAccountDataStore backupAccountDataStore,
            Func<string> getDataStoreType)
        {
            this.primaryAccountDataStore = primaryAccountDataStore;
            this.backupAccountDataStore = backupAccountDataStore;
            this.getDataStoreType = getDataStoreType;
        }

        public PaymentService() : this(
            new AccountDataStore(),
            new BackupAccountDataStore(),
            () => ConfigurationManager.AppSettings["DataStoreType"]
        )
        {
        }
        
        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var dataStoreType = this.getDataStoreType();

            var factory = new AccountDataStoreFactory(this.backupAccountDataStore, this.primaryAccountDataStore);
            var accountDataStore = factory.GetDataStore(dataStoreType);
            Account account = accountDataStore.GetAccount(request.DebtorAccountNumber);

            var result = new MakePaymentResult();
            result.Success = true;
            
            if (account == null)
            {
                result.Success = false;
                return result;
            }
            
            switch (request.PaymentScheme)
            {
                case PaymentScheme.Bacs:
                    if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs))
                    {
                        result.Success = false;
                        return result;
                    }
                    break;

                case PaymentScheme.FasterPayments:
                    if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
                    {
                        result.Success = false;
                    }
                    if (account.Balance < request.Amount)
                    {
                        result.Success = false;
                    }
                    break;

                case PaymentScheme.Chaps:
                    if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
                    {
                        result.Success = false;
                    }
                    if (account.Status != AccountStatus.Live)
                    {
                        result.Success = false;
                    }
                    break;
            }

            if (result.Success)
            {
                account.Balance -= request.Amount;

                if (dataStoreType == "Backup")
                {
                    accountDataStore.UpdateAccount(account);
                }
                else
                {
                    accountDataStore.UpdateAccount(account);
                }
            }
            
            return result;
        }
    }
}
