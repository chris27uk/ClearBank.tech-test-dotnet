### Test Description
In the 'PaymentService.cs' file you will find a method for making a payment. At a high level the steps for making a payment are:

 - Lookup the account the payment is being made from
 - Check the account is in a valid state to make the payment
 - Deduct the payment amount from the account's balance and update the account in the database
 
What we’d like you to do is refactor the code with the following things in mind:  
 - Adherence to SOLID principals
 - Testability  
 - Readability 

We’d also like you to add some unit tests to the ClearBank.DeveloperTest.Tests project to show how you would test the code that you’ve produced. The only specific ‘rules’ are:  

 - The solution should build.
 - The tests should all pass.
 - You should not change the method signature of the MakePayment method.

You are free to use any frameworks/NuGet packages that you see fit.  
 
You should plan to spend around 1 to 3 hours to complete the exercise.

# Chris G - 2025-10-23
## Changes

- Isolated data-store selection — Injected Func<string> into PaymentService to decouple from ConfigurationManager. Assumed the data store type might change dynamically (risk-averse choice for a payments system, outside chance of using something like `AppSettingsSection` to drive from DB). If static, this could be simplified to a plain string.

- Laid down test scaffolding — Centralised SUT and doubles creation from day 1 to keep refactorability. This follows [an approach I have blogged about and used for a number of years](https://medium.com/@brumchris/rethinking-the-common-over-use-of-the-builder-pattern-in-c-fast-tests-eddebcd61e77) to scaling that I think works really well. I rarely use mocking libraries as [I find they make reading tests difficult](https://medium.com/@brumchris/libraries-for-mocking-are-bad-19da850adda9) at higher complexities.  I find this combination of approaches is a good DRY balance in tests.
