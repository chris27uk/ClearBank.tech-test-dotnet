using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Features.MakePayment
{
    public interface IPaymentService
    {
        MakePaymentResult MakePayment(MakePaymentRequest request);
    }
}
