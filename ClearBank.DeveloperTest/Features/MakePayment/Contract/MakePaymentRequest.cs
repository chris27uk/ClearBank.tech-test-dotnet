using System;

// ReSharper disable once CheckNamespace
namespace ClearBank.DeveloperTest.Types
{
    public class MakePaymentRequest
    {
        public string CreditorAccountNumber { get; set; }

        public string DebtorAccountNumber { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public PaymentScheme PaymentScheme { get; set; }

        public bool IsUnknownPaymentScheme() => !Enum.IsDefined(typeof(PaymentScheme), this.PaymentScheme);
    }
}
