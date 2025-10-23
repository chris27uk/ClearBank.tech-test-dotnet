using ClearBank.DeveloperTest.Features.MakePayment.Accounts;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Features.MakePayment.Payment
{
    public class FasterPaymentsPayment(Account account) : IPayment
    {
        public bool DebtorCanPay(MakePaymentRequest request)
        {
            return account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments) &&
                   account.Balance >= request.Amount;
        }
    }
}
