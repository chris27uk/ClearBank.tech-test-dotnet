using ClearBank.DeveloperTest.Features.MakePayment.Accounts;
using ClearBank.DeveloperTest.Features.MakePayment.Contract;
using ClearBank.DeveloperTest.Tests.Infrastructure;
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
            [
                PaymentTestSubject.CreateAccount(balance: 50, allowedPaymentSchemes: allowedPaymentSchemes)
            ]);
            var sut = context.CreatePaymentService();
        
            _ = sut.MakePayment(PaymentTestSubject.CreatePaymentRequest(paymentScheme: scheme, amount: 50));
        
            Assert.That(context.BackupAccountDataStore.Updates, Is.Empty);
        }
        
        [TestCase(PaymentScheme.Bacs, AllowedPaymentSchemes.Bacs)]
        [TestCase(PaymentScheme.Chaps, AllowedPaymentSchemes.Chaps)]
        [TestCase(PaymentScheme.FasterPayments, AllowedPaymentSchemes.FasterPayments)]
        public void Given_A_Valid_Request_When_Paying_Using_Backup_Store_Then_Account_Is_Debited(PaymentScheme scheme, AllowedPaymentSchemes allowedPaymentSchemes)
        {
            var context = PaymentTestSubject.WithExpectedPaymentResponse(
                dataStoreType: "Backup",
                accountsInBackupDataStore:
                [
                    PaymentTestSubject.CreateAccount(balance: 50, allowedPaymentSchemes: allowedPaymentSchemes)
                ]);
            var sut = context.CreatePaymentService();
        
            _ = sut.MakePayment(PaymentTestSubject.CreatePaymentRequest(paymentScheme: scheme, amount: 50));
        
            Assert.That(context.BackupAccountDataStore.Updates.Single().Balance, Is.EqualTo(0));
        }
        
        [TestCase(PaymentScheme.Bacs, AllowedPaymentSchemes.Bacs)]
        [TestCase(PaymentScheme.Chaps, AllowedPaymentSchemes.Chaps)]
        [TestCase(PaymentScheme.FasterPayments, AllowedPaymentSchemes.FasterPayments)]
        public void Given_A_Valid_Request_When_Paying_Using_Backup_Store_Then_Primary_Data_Store_Is_Not_Affected(PaymentScheme scheme, AllowedPaymentSchemes allowedPaymentSchemes)
        {
            var context = PaymentTestSubject.WithExpectedPaymentResponse(
                dataStoreType: "Backup",
                accountsInBackupDataStore:
                [PaymentTestSubject.CreateAccount(balance: 50, allowedPaymentSchemes: allowedPaymentSchemes)]);
            var sut = context.CreatePaymentService();
        
            _ = sut.MakePayment(PaymentTestSubject.CreatePaymentRequest(paymentScheme: scheme, amount: 50));
            
            Assert.That(context.PrimaryAccountDataStore.Updates, Is.Empty);
        }
        
        [TestCase(PaymentScheme.Bacs, AllowedPaymentSchemes.Bacs)]
        [TestCase(PaymentScheme.Chaps, AllowedPaymentSchemes.Chaps)]
        [TestCase(PaymentScheme.FasterPayments, AllowedPaymentSchemes.FasterPayments)]
        public void Given_A_Valid_Payment_Request_For_An_Account_In_Both_Stores_When_Paying_Then_Uses_Correct_Amount(PaymentScheme scheme, AllowedPaymentSchemes allowedPaymentSchemes)
        {
            var backupAccount = PaymentTestSubject.CreateAccount(balance: 50);
            var primaryAccount = PaymentTestSubject.CreateAccount(balance: 30);
            var context = PaymentTestSubject.WithExpectedPaymentResponse(
                dataStoreType: "Backup",
                accountsInBackupDataStore: [ backupAccount ],
                accountsInPrimaryDataStore: [ primaryAccount ]);
            var sut = context.CreatePaymentService();
        
            _ = sut.MakePayment(PaymentTestSubject.CreatePaymentRequest(paymentScheme: scheme, amount: 10));
        
            Assert.That(context.BackupAccountDataStore.Updates.Single().Balance, Is.EqualTo(40));
        }
    }
}
