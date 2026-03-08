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


# Payment Gateway: Responsible for validating requests, storing card information and forwarding payment requests and accepting payment responses to and from the acquiring bank.

#    Services/
#       PaymentGatewayService::IPaymentGateway
     
# Acquiring Bank: Allows us to do the actual retrieval of money from the shopper’s card and pay out to the merchant. It also performs some validation of the card information and then sends the payment details to the  appropriate 3rd party organization for processing.

#    Infrastructure/
#       Banks/
#           AcquiringBank::IAcquiringBank


# **** Notice that it does not call IAcquiringBank if the validation has failed.

# Validator: any new rule, just implement the interface
#   Infrastructure/
#       Validators/
#           Rules/
#             IValidateRule


