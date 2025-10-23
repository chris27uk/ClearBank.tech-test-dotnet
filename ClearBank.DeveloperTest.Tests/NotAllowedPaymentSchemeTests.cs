using ClearBank.DeveloperTest.Tests.Infrastructure;
using ClearBank.DeveloperTest.Types;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests
{
    public class NotAllowedPaymentSchemeTests
    {
        [TestCase(PaymentScheme.Bacs, AllowedPaymentSchemes.FasterPayments)]
        [TestCase(PaymentScheme.Chaps, AllowedPaymentSchemes.FasterPayments)]
        [TestCase(PaymentScheme.FasterPayments, AllowedPaymentSchemes.Bacs)]
        public void Given_A_Payment_Method_Not_Allowed_When_Paying_Then_The_Payment_Is_Not_Successful(PaymentScheme scheme, AllowedPaymentSchemes allowedPaymentSchemes)
        {
            var context = PaymentTestSubject.WithExpectedResponse();
            var sut = context.CreatePaymentService();
        
            var result = sut.MakePayment(PaymentTestSubject.CreatePaymentRequest(paymentScheme: scheme));
        
            Assert.That(result.Success, Is.False);
        }
    }
}
