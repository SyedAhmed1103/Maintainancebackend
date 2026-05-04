using Microsoft.AspNetCore.Mvc;
using Maintainancebackend.Models;

[ApiController]
[Route("api/[controller]")]
public class UserFlatsController : ControllerBase
{
    private readonly UserFlatService _service;

    public UserFlatsController(UserFlatService service)
    {
        _service = service;
    }

    // GET: api/userflats?buildingId=1
    [HttpGet]
    public async Task<IActionResult> GetAll(int buildingId)
    {
        var data = await _service.GetAllAsync(buildingId);
        return Ok(data);
    }

    // GET: api/userflats/user/5?buildingId=1
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(int userId, int buildingId)
    {
        var data = await _service.GetByUserAsync(userId, buildingId);
        return Ok(data);
    }

    // POST
    [HttpPost]
    public async Task<IActionResult> Create(UserFlat f)
    {
        await _service.CreateAsync(f);
        return Ok("Created");
    }

    // DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, int buildingId)
    {
        await _service.DeleteAsync(id, buildingId);
        return Ok("Deleted");
    }
}