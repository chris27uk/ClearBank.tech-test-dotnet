using System;

namespace ClearBank.DeveloperTest.Features.MakePayment.Contract
{
    public class MakePaymentRequest
    {
        public string CreditorAccountNumber { get; set; }

        public string DebtorAccountNumber { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public PaymentScheme PaymentScheme { get; set; }
    }
}
