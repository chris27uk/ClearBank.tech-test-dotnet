using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Features.MakePayment.Payment
{
    public interface IPayment
    {
        bool Validate(MakePaymentRequest request);
    }
}
