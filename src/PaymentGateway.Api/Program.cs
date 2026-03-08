using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Diagnostics;

using PaymentGateway.Api.Infrastructure.Banks;
using PaymentGateway.Api.Infrastructure.Helpers;
using PaymentGateway.Api.Infrastructure.Repositories;
using PaymentGateway.Api.Infrastructure.Validators;
using PaymentGateway.Api.Infrastructure.Validators.Rules;
using PaymentGateway.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//From the spec it was requiring to export as Declined/Authorised/Rejected
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//existents services
builder.Services.AddSingleton<PaymentsRepository>();

//Infra structure Services
builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddSingleton<IPaymentsRepository, PaymentsRepositoryProxy>();
builder.Services.AddSingleton<IValidatorService, ValidatorService>();
builder.Services.AddSingleton<IValidateRule, ValidateAmount>();
builder.Services.AddSingleton<IValidateRule, ValidateCardNumber>();
builder.Services.AddSingleton<IValidateRule, ValidateCvv>();
builder.Services.AddSingleton<IValidateRule, ValidateExpiry>();
builder.Services.AddSingleton<IValidateRule, ValidateCurrency>();
builder.Services.AddHttpClient<IAcquiringBank, AcquiringBank>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BankSimulator:BaseUrl"]!);
    client.Timeout = TimeSpan.FromSeconds(10);
});
//Payment Orchestrator: the Payment gateway
builder.Services.AddScoped<IPaymentGateway, PaymentGatewayService>();

var app = builder.Build();

// good practice adding this middlewere ..  no needs try catch on controllers.. more cpu friendly
app.UseExceptionHandler(app => app.Run(async context =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

    logger.LogError(exception, "Unhandled exception on {Method} {Path}",
        context.Request.Method,
        context.Request.Path);

    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred." });
}));


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
