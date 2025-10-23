using ClearBank.DeveloperTest.Features.MakePayment.Accounts;
using ClearBank.DeveloperTest.Tests.Infrastructure;
using ClearBank.DeveloperTest.Types;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Validation
{
    public class NotAllowedPaymentSchemeTests
    {
        [TestCase(PaymentScheme.Bacs, AllowedPaymentSchemes.FasterPayments)]
        [TestCase(PaymentScheme.Chaps, AllowedPaymentSchemes.FasterPayments)]
        [TestCase(PaymentScheme.FasterPayments, AllowedPaymentSchemes.Bacs)]
        public void Given_A_Payment_Method_Not_Allowed_When_Paying_Then_The_Payment_Is_Not_Successful(PaymentScheme scheme, AllowedPaymentSchemes allowedPaymentSchemes)
        {
            var context = SingleAccountWithSpecificAllowedPaymentSchemes(allowedPaymentSchemes);
            var sut = context.CreatePaymentService();
        
            var result = sut.MakePayment(PaymentTestSubject.CreatePaymentRequest(paymentScheme: scheme));
        
            Assert.That(result.Success, Is.False);
        }

        [TestCase(PaymentScheme.Bacs, AllowedPaymentSchemes.FasterPayments)]
        [TestCase(PaymentScheme.Chaps, AllowedPaymentSchemes.FasterPayments)]
        [TestCase(PaymentScheme.FasterPayments, AllowedPaymentSchemes.Bacs)]
        public void Given_A_Payment_Method_Not_Allowed_When_Paying_Then_The_Account_Is_Not_Updated(PaymentScheme scheme, AllowedPaymentSchemes allowedPaymentSchemes)
        {
            var context = SingleAccountWithSpecificAllowedPaymentSchemes(allowedPaymentSchemes);
            var sut = context.CreatePaymentService();
        
            _ = sut.MakePayment(PaymentTestSubject.CreatePaymentRequest(paymentScheme: scheme));
        
            Assert.That(context.PrimaryAccountDataStore.Updates, Is.Empty);
            Assert.That(context.BackupAccountDataStore.Updates, Is.Empty);
        }
        
        private static PaymentTestSubject SingleAccountWithSpecificAllowedPaymentSchemes(AllowedPaymentSchemes allowedPaymentSchemes)
        {
            return PaymentTestSubject.WithExpectedPaymentResponse(accountsInPrimaryDataStore: [ PaymentTestSubject.CreateAccount(allowedPaymentSchemes: allowedPaymentSchemes) ]);
        }
    }
}
