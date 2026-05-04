using Microsoft.AspNetCore.Mvc;
using Maintainancebackend.Models;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly PaymentService _service;

    public PaymentsController(PaymentService service)
    {
        _service = service;
    }

    // GET: api/payments?buildingId=1
    [HttpGet]
    public async Task<IActionResult> GetAll(int buildingId)
    {
        return Ok(await _service.GetAllAsync(buildingId));
    }

    // GET: api/payments/flat/1?buildingId=1
    [HttpGet("flat/{flatId}")]
    public async Task<IActionResult> GetByFlat(int flatId, int buildingId)
    {
        return Ok(await _service.GetByFlatAsync(flatId, buildingId));
    }

    // POST
    [HttpPost]
    public async Task<IActionResult> Create(Payment p)
    {
        await _service.CreateAsync(p);
        return Ok("Payment Created");
    }

    // MARK PAID
    [HttpPut("{id}/pay")]
    public async Task<IActionResult> MarkPaid(int id, string transactionId, int buildingId)
    {
        await _service.MarkAsPaidAsync(id, transactionId, buildingId);
        return Ok("Marked as Paid");
    }
}