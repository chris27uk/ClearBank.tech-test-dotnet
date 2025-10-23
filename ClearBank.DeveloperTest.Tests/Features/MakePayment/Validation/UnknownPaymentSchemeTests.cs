using ClearBank.DeveloperTest.Tests.Infrastructure;
using ClearBank.DeveloperTest.Types;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Features.MakePayment.Validation
{
    public class UnknownPaymentSchemeTests
    {
        [Test]
        public void Given_An_Unknown_Payment_Scheme_When_Paying_Then_The_Payment_Is_Not_Successful()
        {
            var context = PaymentTestSubject.WithExpectedPaymentResponse(accountsInPrimaryDataStore: []);
            var sut = context.CreatePaymentService();

            var result = sut.MakePayment(PaymentTestSubject.CreatePaymentRequest(paymentScheme: (PaymentScheme)500));
        
            Assert.That(result.Success, Is.False);
        }
    
        [Test]
        public void Given_An_Unknown_Payment_Scheme_When_Paying_Then_No_Account_Is_Updated()
        {
            var context = PaymentTestSubject.WithExpectedPaymentResponse(accountsInPrimaryDataStore: []);
            var sut = context.CreatePaymentService();

            _ = sut.MakePayment(PaymentTestSubject.CreatePaymentRequest(paymentScheme: (PaymentScheme)500));
        
            Assert.That(context.PrimaryAccountDataStore.Updates, Is.Empty);
            Assert.That(context.BackupAccountDataStore.Updates, Is.Empty);
        }
    }
}
