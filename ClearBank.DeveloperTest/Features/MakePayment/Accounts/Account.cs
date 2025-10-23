﻿namespace ClearBank.DeveloperTest.Features.MakePayment.Accounts
{
    public class Account
    {
        // Not made immutable in case it is the DTO for the database.
        
        public string AccountNumber { get; set; }
        
        public decimal Balance { get; set; }
        
        public void Debit(decimal amount) => Balance -= amount;
        
        public AccountStatus Status { get; set; }
        
        public AllowedPaymentSchemes AllowedPaymentSchemes { get; set; }
    }
}
