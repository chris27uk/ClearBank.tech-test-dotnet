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

- Made data layer mockable — Introduced AccountDataStoreFactory and IAccountDataStore so data stores can be substituted in tests.


- Added a behaviour-focused test pack — Tests assert behaviours not collaborators or internals. No mocking library; three-phase tests with frequent commits and coverage checks.


- Restructured files for clarity — Switched from functional folders to a clean-architecture split: visible entrypoint, use cases, infra, and contracts.


- Refactored to a payment strategy pattern — Moved SRP-violating logic into scheme-specific payment objects invoked by the service. This buys clarity and rate-of-change isolation. A YAGNI/KISS pushback is plausible; in reality I would justify against product vision/backlog context.

## If I had more time

- I would put more testing effort into the backup data store. While absolutely not
  needed for the refactoring safety, it would be good to validate this in case the logic becomes less simple.
