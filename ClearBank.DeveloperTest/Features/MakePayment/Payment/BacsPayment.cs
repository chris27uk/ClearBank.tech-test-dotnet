using ClearBank.DeveloperTest.Features.MakePayment.Accounts;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Features.MakePayment.Payment
{
    public class BacsPayment(Account account) : IPayment
    {
        public bool Validate(MakePaymentRequest request) => account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs);
    }
}
