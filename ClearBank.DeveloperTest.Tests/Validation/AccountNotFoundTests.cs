using ClearBank.DeveloperTest.Tests.Infrastructure;
using ClearBank.DeveloperTest.Types;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Validation
{
    public class AccountNotFoundTests
    {
        [TestCase(PaymentScheme.FasterPayments, AllowedPaymentSchemes.FasterPayments)]
        [TestCase(PaymentScheme.Bacs, AllowedPaymentSchemes.Bacs)]
        [TestCase(PaymentScheme.Chaps, AllowedPaymentSchemes.Bacs)]
        public void Given_An_Account_With_No_Account_When_Paying_Then_The_Payment_Is_Not_Successful(PaymentScheme scheme, AllowedPaymentSchemes allowedPaymentSchemes)
        {
            var context = PaymentTestSubject.WithExpectedResponse(accountsInPrimaryDataStore: []);
            var sut = context.CreatePaymentService();
        
            var result = sut.MakePayment(PaymentTestSubject.CreatePaymentRequest(paymentScheme: scheme));
        
            Assert.That(result.Success, Is.False);
        }
    
        [Test]
        public void Given_An_Account_With_No_Credit_When_Paying_Faster_Payments_Then_No_Account_Is_Updated()
        {
            var context = PaymentTestSubject.WithExpectedResponse(accountsInPrimaryDataStore:
            [
                PaymentTestSubject.CreateAccount(balance: 0)
            ]);
            var sut = context.CreatePaymentService();
        
            _ = sut.MakePayment(PaymentTestSubject.CreatePaymentRequest(paymentScheme: PaymentScheme.FasterPayments));
        
            Assert.That(context.PrimaryAccountDataStore.Updates, Is.Empty);
            Assert.That(context.BackupAccountDataStore.Updates, Is.Empty);
        }
    }
}
