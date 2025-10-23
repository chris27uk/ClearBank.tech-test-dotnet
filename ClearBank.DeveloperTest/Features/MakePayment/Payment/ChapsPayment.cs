using ClearBank.DeveloperTest.Features.MakePayment.Accounts;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Features.MakePayment.Payment
{
    public class ChapsPayment(Account account) : IPayment
    {
        public bool DebtorCanPay(MakePaymentRequest request)
        {
            return account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps) &&
                   account.Status == AccountStatus.Live;
        }
    }
}
