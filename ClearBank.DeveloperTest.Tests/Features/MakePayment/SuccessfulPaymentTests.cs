using ClearBank.DeveloperTest.Tests.Infrastructure;
using ClearBank.DeveloperTest.Types;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Features.MakePayment
{
    public class SuccessfulPaymentTests
    {
        [TestCase(PaymentScheme.Bacs, AllowedPaymentSchemes.Bacs)]
        [TestCase(PaymentScheme.Chaps, AllowedPaymentSchemes.Chaps)]
        [TestCase(PaymentScheme.FasterPayments, AllowedPaymentSchemes.FasterPayments)]
        public void Given_A_Valid_Request_When_Paying_Then_Is_Successful(PaymentScheme scheme, AllowedPaymentSchemes allowedPaymentSchemes)
        {
            var context = PaymentTestSubject.WithExpectedPaymentResponse(accountsInPrimaryDataStore:
            [
                PaymentTestSubject.CreateAccount(allowedPaymentSchemes: allowedPaymentSchemes)
            ]);
            var sut = context.CreatePaymentService();

            var result = sut.MakePayment(PaymentTestSubject.CreatePaymentRequest(paymentScheme: scheme));

            Assert.That(result.Success, Is.True);
        }
    }
}
