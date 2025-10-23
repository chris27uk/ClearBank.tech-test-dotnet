using ClearBank.DeveloperTest.Features.MakePayment.Contract;

namespace ClearBank.DeveloperTest.Features.MakePayment
{
    public interface IPaymentService
    {
        MakePaymentResult MakePayment(MakePaymentRequest request);
    }
}
