using ClearBank.DeveloperTest.Tests.Infrastructure;
using ClearBank.DeveloperTest.Types;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Validation.NoCreditTests
{
    public class FasterPaymentNoCreditTests
    {
        [Test]
        public void Given_An_Account_With_No_Credit_When_Paying_With_Faster_Payments_Then_The_Payment_Is_Not_Successful()
        {
            var context = PaymentTestSubject.WithExpectedResponse(accountsInPrimaryDataStore:
            [
                PaymentTestSubject.CreateAccount(balance: 0, allowedPaymentSchemes: AllowedPaymentSchemes.FasterPayments)
            ]);
            var sut = context.CreatePaymentService();
        
            var result = sut.MakePayment(PaymentTestSubject.CreatePaymentRequest(paymentScheme: PaymentScheme.FasterPayments));
        
            Assert.That(result.Success, Is.False);
        }
    
        [Test]
        public void Given_An_Account_With_No_Credit_When_Paying_With_Faster_Payments_Then_No_Account_Is_Updated()
        {
            var context = PaymentTestSubject.WithExpectedResponse(accountsInPrimaryDataStore:
            [
                PaymentTestSubject.CreateAccount(balance: 0, allowedPaymentSchemes: AllowedPaymentSchemes.FasterPayments)
            ]);
            var sut = context.CreatePaymentService();
        
            _ = sut.MakePayment(PaymentTestSubject.CreatePaymentRequest(paymentScheme: PaymentScheme.FasterPayments));
        
            Assert.That(context.PrimaryAccountDataStore.Updates, Is.Empty);
            Assert.That(context.BackupAccountDataStore.Updates, Is.Empty);
        }
    }
}
