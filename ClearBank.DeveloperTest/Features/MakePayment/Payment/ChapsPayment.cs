using ClearBank.DeveloperTest.Features.MakePayment.Accounts;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Features.MakePayment.Payment
{
    public class ChapsPayment(Account account) : IPayment
    {
        public bool DebtorCanPay(MakePaymentRequest request)
        {
            if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
            {
                return false;
            }

            if (account.Status != AccountStatus.Live)
            {
                return false;
            }

            return true;
        }
    }
}
