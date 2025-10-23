using System;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Tests.Infrastructure
{
    public class PaymentTestSubject
    {
        private readonly string dataStoreType;

        private PaymentTestSubject(string dataStoreType) => this.dataStoreType = dataStoreType;
        
        public PaymentService CreatePaymentService(string dataStoreType = "Default") => new(() => dataStoreType);

        public static readonly DateTime DefaultPaymentDate = new(2022, 1, 1);
    
        public static PaymentTestSubject WithExpectedResponse(
            Account[] accountsInPrimaryDataStore = null,
            string dataStoreType = "Default",
            Account[] accountsInBackupDataStore = null) =>
            new(dataStoreType);
        
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
