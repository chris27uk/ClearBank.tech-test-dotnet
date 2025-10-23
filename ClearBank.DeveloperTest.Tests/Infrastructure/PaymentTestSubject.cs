using System;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Tests.Infrastructure
{
    public class PaymentTestSubject
    {
        private readonly string dataStoreType;

        private PaymentTestSubject(FakeAccountDataStore primaryAccountDataStore, FakeAccountDataStore backupAccountDataStore, string dataStoreType)
        {
            this.PrimaryAccountDataStore = primaryAccountDataStore;
            this.BackupAccountDataStore = backupAccountDataStore;
            this.dataStoreType = dataStoreType;
        }

        public FakeAccountDataStore PrimaryAccountDataStore { get; }

        public FakeAccountDataStore BackupAccountDataStore { get; }

        public PaymentService CreatePaymentService() => new(this.PrimaryAccountDataStore, this.BackupAccountDataStore, () => dataStoreType);

        public static readonly DateTime DefaultPaymentDate = new(2022, 1, 1);
    
        public static PaymentTestSubject WithExpectedResponse(
            Account[] accountsInPrimaryDataStore = null,
            string dataStoreType = "Default",
            Account[] accountsInBackupDataStore = null) =>
            new(new FakeAccountDataStore(accountsInPrimaryDataStore ?? []), new FakeAccountDataStore(accountsInBackupDataStore ?? []), dataStoreType);
        
        public static MakePaymentRequest CreatePaymentRequest(
            decimal amount = 100, 
            string creditorAccountNumber = "1234567890",
            string debtorAccountNumber = "1234567890",
            DateTime? paymentDate = null,
            PaymentScheme paymentScheme = PaymentScheme.FasterPayments)
        {
            return new MakePaymentRequest
            {
                Amount = amount,
                DebtorAccountNumber = debtorAccountNumber,
                CreditorAccountNumber = creditorAccountNumber,
                PaymentDate = paymentDate ?? DefaultPaymentDate,
                PaymentScheme = paymentScheme
            };
        }
    }
}
