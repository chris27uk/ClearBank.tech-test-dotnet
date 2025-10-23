using ClearBank.DeveloperTest.Features.MakePayment.Accounts;
using ClearBank.DeveloperTest.Features.MakePayment.Payment;
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
            var accountDataStore = GetAccountDataStore();
            var account = accountDataStore.GetAccount(request.DebtorAccountNumber);
            var payment = CreatePayment(request, account);

            if (account == null || request.IsUnknownPaymentScheme() || !payment.DebtorCanPay(request))
            {
                return MakePaymentResult.ForFailure();
            }

            account.Debit(request.Amount);
            accountDataStore.UpdateAccount(account);
            return MakePaymentResult.ForSuccess();
        }

        private IAccountDataStore GetAccountDataStore()
        {
            var dataStoreType = this.getDataStoreType();
            var factory = new AccountDataStoreFactory(this.backupAccountDataStore, this.primaryAccountDataStore);
            var accountDataStore = factory.GetDataStore(dataStoreType);
            return accountDataStore;
        }

        private static IPayment CreatePayment(MakePaymentRequest request, Account account)
        {
            return request.PaymentScheme switch
            {
                PaymentScheme.Bacs => new BacsPayment(account),
                PaymentScheme.FasterPayments => new FasterPaymentsPayment(account),
                PaymentScheme.Chaps => new ChapsPayment(account),
                _ => null
            };
        }
    }
}
