using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Api.Models;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;
using PaymentGateway.Api.Services;

namespace PaymentGateway.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController : Controller
{
    private readonly ILogger<PaymentsController> _logger;
    private readonly PaymentsRepository _paymentsRepository;
    private readonly IPaymentGateway _paymentGateway;

    public PaymentsController(ILogger<PaymentsController> logger,
                              PaymentsRepository paymentsRepository,
                              IPaymentGateway paymentGateway)
    {
        _logger = logger;
        _paymentsRepository = paymentsRepository;
        _paymentGateway = paymentGateway;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PostPaymentResponse?>> GetPaymentAsync(Guid id)
    {
        var payment = _paymentsRepository.Get(id);

        return new OkObjectResult(payment);
    }

    [HttpPost]
    [ProducesResponseType(typeof(BankResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BankResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<BankResponse?>> ProcessPayment([FromBody] PostPaymentRequestModel request,
                                                    CancellationToken cancellationToken)
    {
        var result = await _paymentGateway.ProcessAsync(request.ToProcessPaymentRequest(), cancellationToken);
        return result.Status == PaymentStatus.Rejected
                ? UnprocessableEntity(result.ToPostPaymentResponse())
                : Ok(result.ToPostPaymentResponse());
    }
}