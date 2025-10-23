using ClearBank.DeveloperTest.Features.MakePayment.Accounts;
using ClearBank.DeveloperTest.Tests.Infrastructure;
using ClearBank.DeveloperTest.Types;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Features.MakePayment.Validation
{
    public class AccountNotLiveTests
    {
        [TestCase(AccountStatus.Disabled)]
        [TestCase(AccountStatus.InboundPaymentsOnly)]
        public void Given_An_Non_Live_Account_When_Paying_With_Chaps_Then_No_Account_Is_Updated(AccountStatus status)
        {
            var context = WithSingleAccountInPrimaryDataStore(status);
            var sut = context.CreatePaymentService();

            _ = sut.MakePayment(PaymentTestSubject.CreatePaymentRequest(paymentScheme: PaymentScheme.Chaps));

            Assert.That(context.PrimaryAccountDataStore.Updates, Is.Empty);
            Assert.That(context.BackupAccountDataStore.Updates, Is.Empty);
        }

        [TestCase(AccountStatus.Disabled)]
        [TestCase(AccountStatus.InboundPaymentsOnly)]
        public void Given_An_Non_Live_Account_When_Paying_With_Chaps_Then_Payment_Is_Not_Successful(
            AccountStatus status)
        {
            var context = WithSingleAccountInPrimaryDataStore(status);
            var sut = context.CreatePaymentService();

            var result = sut.MakePayment(PaymentTestSubject.CreatePaymentRequest(paymentScheme: PaymentScheme.Chaps));

            Assert.That(result.Success, Is.False);
        }

        [TestCase(PaymentScheme.Bacs, AccountStatus.Disabled)]
        [TestCase(PaymentScheme.Bacs, AccountStatus.InboundPaymentsOnly)]
        [TestCase(PaymentScheme.FasterPayments, AccountStatus.Disabled)]
        [TestCase(PaymentScheme.FasterPayments, AccountStatus.InboundPaymentsOnly)]
        public void Given_An_Non_Live_Account_When_Paying_With_Other_Payment_Schemes_Then_Account_Is_Updated(
            PaymentScheme scheme, AccountStatus status)
        {
            var context = WithSingleAccountInPrimaryDataStore(status);
            var sut = context.CreatePaymentService();

            _ = sut.MakePayment(PaymentTestSubject.CreatePaymentRequest(paymentScheme: scheme));

            Assert.That(context.PrimaryAccountDataStore.Updates, Has.One.Items);
        }

        [TestCase(PaymentScheme.Bacs, AccountStatus.Disabled)]
        [TestCase(PaymentScheme.Bacs, AccountStatus.InboundPaymentsOnly)]
        [TestCase(PaymentScheme.FasterPayments, AccountStatus.Disabled)]
        [TestCase(PaymentScheme.FasterPayments, AccountStatus.InboundPaymentsOnly)]
        public void Given_An_Non_Live_Account_When_Paying_With_Other_Payment_Schemes_Then_Payment_Is_Successful(
            PaymentScheme scheme, AccountStatus status)
        {
            var context = WithSingleAccountInPrimaryDataStore(status);
            var sut = context.CreatePaymentService();

            var result = sut.MakePayment(PaymentTestSubject.CreatePaymentRequest(paymentScheme: scheme));

            Assert.That(result.Success, Is.True);
        }
        
        private static PaymentTestSubject WithSingleAccountInPrimaryDataStore(AccountStatus status) => PaymentTestSubject.WithExpectedPaymentResponse(accountsInPrimaryDataStore:
        [
            PaymentTestSubject.CreateAccount(status: status)
        ]);
    }
}
