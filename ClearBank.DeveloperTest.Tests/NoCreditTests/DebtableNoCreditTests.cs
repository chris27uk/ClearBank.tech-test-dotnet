using ClearBank.DeveloperTest.Tests.Infrastructure;
using ClearBank.DeveloperTest.Types;
using NUnit.Framework;
using System.Linq;

namespace ClearBank.DeveloperTest.Tests.NoCreditTests
{
    public class DebtableNoCreditTests
    {
        [TestCase(PaymentScheme.Bacs, AllowedPaymentSchemes.Bacs)]
        [TestCase(PaymentScheme.Chaps, AllowedPaymentSchemes.Chaps)]
        public void Given_An_Account_With_No_Credit_When_Paying_With_Scheme_That_Allows_Debt_Then_Account_Is_Updated(PaymentScheme scheme, AllowedPaymentSchemes allowedPaymentSchemes)
        {
            var context = PaymentTestSubject.WithExpectedResponse(accountsInPrimaryDataStore:
            [
                PaymentTestSubject.CreateAccount(balance: 0, allowedPaymentSchemes: allowedPaymentSchemes)
            ]);
            var sut = context.CreatePaymentService();
        
            _ = sut.MakePayment(PaymentTestSubject.CreatePaymentRequest(paymentScheme: scheme, amount: 50));
        
            Assert.That(context.PrimaryAccountDataStore.Updates.Single().Balance, Is.EqualTo(-50) );
            Assert.That(context.BackupAccountDataStore.Updates, Is.Empty);
        }

        [TestCase(PaymentScheme.Bacs, AllowedPaymentSchemes.Bacs)]
        [TestCase(PaymentScheme.Chaps, AllowedPaymentSchemes.Chaps)]
        public void Given_An_Account_With_No_Credit_When_Paying_With_Scheme_That_Allows_Debt_Then_Succeeds(
            PaymentScheme scheme, AllowedPaymentSchemes allowedPaymentSchemes)
        {
            var context = PaymentTestSubject.WithExpectedResponse(accountsInPrimaryDataStore:
            [
                PaymentTestSubject.CreateAccount(balance: 0, allowedPaymentSchemes: allowedPaymentSchemes)
            ]);
            var sut = context.CreatePaymentService();

            var result = sut.MakePayment(PaymentTestSubject.CreatePaymentRequest(paymentScheme: scheme, amount: 50));

            Assert.That(result.Success, Is.True);
        }
    }
}
