using ClearBank.DeveloperTest.Tests.Infrastructure;
using ClearBank.DeveloperTest.Types;
using NUnit.Framework;
using System.Linq;

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
        
        [TestCase(PaymentScheme.Bacs, AllowedPaymentSchemes.Bacs)]
        [TestCase(PaymentScheme.Chaps, AllowedPaymentSchemes.Chaps)]
        [TestCase(PaymentScheme.FasterPayments, AllowedPaymentSchemes.FasterPayments)]
        public void Given_A_Valid_Request_When_Paying_Then_Account_Is_Debited(PaymentScheme scheme, AllowedPaymentSchemes allowedPaymentSchemes)
        {
            var context = PaymentTestSubject.WithExpectedPaymentResponse(accountsInPrimaryDataStore:
            [
                PaymentTestSubject.CreateAccount(balance: 50, allowedPaymentSchemes: allowedPaymentSchemes)
            ]);
            var sut = context.CreatePaymentService();
        
            _ = sut.MakePayment(PaymentTestSubject.CreatePaymentRequest(paymentScheme: scheme, amount: 50));
        
            Assert.That(context.PrimaryAccountDataStore.Updates.Single().Balance, Is.EqualTo(0));
        }
        
        [TestCase(PaymentScheme.Bacs, AllowedPaymentSchemes.Bacs)]
        [TestCase(PaymentScheme.Chaps, AllowedPaymentSchemes.Chaps)]
        [TestCase(PaymentScheme.FasterPayments, AllowedPaymentSchemes.FasterPayments)]
        public void Given_A_Valid_Request_When_Paying_Using_Primary_Store_Then_Backup_Data_Store_Is_Not_Affected(PaymentScheme scheme, AllowedPaymentSchemes allowedPaymentSchemes)
        {
            var context = PaymentTestSubject.WithExpectedPaymentResponse(accountsInBackupDataStore:
                [PaymentTestSubject.CreateAccount(balance: 50, allowedPaymentSchemes: allowedPaymentSchemes)]);
            var sut = context.CreatePaymentService();
        
            _ = sut.MakePayment(PaymentTestSubject.CreatePaymentRequest(paymentScheme: scheme, amount: 50));
        
            Assert.That(context.BackupAccountDataStore.Updates, Is.Empty);
        }
    }
}
