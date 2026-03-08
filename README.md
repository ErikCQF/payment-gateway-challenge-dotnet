# Instructions for candidates

This is the .NET version of the Payment Gateway challenge. If you haven't already read this [README.md](https://github.com/cko-recruitment/) on the details of this exercise, please do so now. 

## Template structure
```
src/
    PaymentGateway.Api - a skeleton ASP.NET Core Web API
test/
    PaymentGateway.Api.Tests - an empty xUnit test project
imposters/ - contains the bank simulator configuration. Don't change this

.editorconfig - don't change this. It ensures a consistent set of rules for submissions when reformatting code
docker-compose.yml - configures the bank simulator
PaymentGateway.sln
```

## Architecture

The solution maps directly to the entities described in the spec:

**Payment Gateway** — validates requests, stores payment records, forwards to the acquiring bank.
```
Services/
  PaymentGatewayService : IPaymentGateway
```

**Acquiring Bank** — called only when validation passes. Handles the actual payment processing.
```
Infrastructure/
               Banks/
                     AcquiringBank : IAcquiringBank
```
> `IAcquiringBank` is never called if validation has failed — this is a hard spec requirement.

**Validator** — chain of responsibility pattern. Each rule is independent and isolated. To add a new rule, implement `IValidateRule` and register it in `Program.cs` — no other changes needed.
```
Infrastructure/
              Validators/
                         Rules/
                               IValidateRule
```

---

