using Microsoft.AspNetCore.Mvc;
using Maintainancebackend.Models;

[ApiController]
[Route("api/[controller]")]
public class NoticesController : ControllerBase
{
    private readonly NoticeService _service;

    public NoticesController(NoticeService service)
    {
        _service = service;
    }

    // GET: api/notices?buildingId=1
    [HttpGet]
    public async Task<IActionResult> GetAll(int buildingId)
    {
        return Ok(await _service.GetAllAsync(buildingId));
    }

    // POST
    [HttpPost]
    public async Task<IActionResult> Create(Notice n)
    {
        await _service.CreateAsync(n);
        return Ok("Notice Created");
    }

    // DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, int buildingId)
    {
        await _service.DeleteAsync(id, buildingId);
        return Ok("Deleted");
    }
}