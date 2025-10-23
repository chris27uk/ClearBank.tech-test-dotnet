﻿using ClearBank.DeveloperTest.Features.MakePayment.Accounts;
using ClearBank.DeveloperTest.Infrastructure.Accounts;
using ClearBank.DeveloperTest.Types;
using System;
using System.Configuration;

namespace ClearBank.DeveloperTest.Features.MakePayment
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStore primaryAccountDataStore;
        private readonly IAccountDataStore backupAccountDataStore;
        private readonly Func<string> getDataStoreType;

        public PaymentService(IAccountDataStore primaryAccountDataStore,
            IAccountDataStore backupAccountDataStore,
            Func<string> getDataStoreType)
        {
            this.primaryAccountDataStore = primaryAccountDataStore;
            this.backupAccountDataStore = backupAccountDataStore;
            this.getDataStoreType = getDataStoreType;
        }

        public PaymentService() : this(
            new AccountDataStore(),
            new BackupAccountDataStore(),
            () => ConfigurationManager.AppSettings["DataStoreType"]
        )
        {
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var dataStoreType = this.getDataStoreType();

            var factory = new AccountDataStoreFactory(this.backupAccountDataStore, this.primaryAccountDataStore);
            var accountDataStore = factory.GetDataStore(dataStoreType);
            Account account = accountDataStore.GetAccount(request.DebtorAccountNumber);

            if (account == null)
            {
                return MakePaymentResult.ForFailure();
            }

            switch (request.PaymentScheme)
            {
                case PaymentScheme.Bacs:
                    if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs))
                    {
                        return MakePaymentResult.ForFailure();
                    }
                    break;

                case PaymentScheme.FasterPayments:
                    if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
                    {
                        return MakePaymentResult.ForFailure();
                    }

                    if (account.Balance < request.Amount)
                    {
                        return MakePaymentResult.ForFailure();
                    }
                    break;

                case PaymentScheme.Chaps:
                    if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
                    {
                        return MakePaymentResult.ForFailure();
                    }

                    if (account.Status != AccountStatus.Live)
                    {
                        return MakePaymentResult.ForFailure();
                    }
                    break;
            }

            account.Balance -= request.Amount;

            if (dataStoreType == "Backup")
            {
                accountDataStore.UpdateAccount(account);
            }
            else
            {
                accountDataStore.UpdateAccount(account);
            }

            return MakePaymentResult.ForSuccess();
        }
    }
}
