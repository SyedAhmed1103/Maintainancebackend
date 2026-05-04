using Microsoft.AspNetCore.Mvc;
using Maintainancebackend.Models;

[ApiController]
[Route("api/[controller]")]
public class FundController : ControllerBase
{
    private readonly FundService _service;

    public FundController(FundService service)
    {
        _service = service;
    }

    // GET: api/fund?buildingId=1
    [HttpGet]
    public async Task<IActionResult> GetAll(int buildingId)
    {
        return Ok(await _service.GetAllAsync(buildingId));
    }

    // POST (ADD ONLY)
    [HttpPost]
    public async Task<IActionResult> Create(Fund f)
    {
        await _service.CreateAsync(f);
        return Ok("Fund entry created");
    }
}