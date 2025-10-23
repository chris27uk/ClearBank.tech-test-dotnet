using ClearBank.DeveloperTest.Features.MakePayment.Accounts;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Features.MakePayment.Payment
{
    public class FasterPaymentsPayment(Account account) : IPayment
    {
        public bool Validate(MakePaymentRequest request)
        {
            if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
            {
                return false;
            }

            if (account.Balance < request.Amount)
            {
                return false;
            }

            return true;
        }
    }
}
