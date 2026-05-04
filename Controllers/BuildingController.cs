using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BuildingController : ControllerBase
{
    private readonly BuildingService _service;

    public BuildingController(BuildingService service)
    {
        _service = service;
    }

    private bool IsValidCode(string accessCode)
    {
        return accessCode == "SECRET123";
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_service.GetAll());
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var data = _service.GetById(id);
        if (data == null) return NotFound();
        return Ok(data);
    }

    [HttpPost]
    public IActionResult Create(Building b, string accessCode)
    {
        if (!IsValidCode(accessCode))
            return Unauthorized("Invalid access code");

        _service.Create(b);
        return Ok("Created");
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Building b, string accessCode)
    {
        if (!IsValidCode(accessCode))
            return Unauthorized("Invalid access code");

        _service.Update(id, b);
        return Ok("Updated");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id, string accessCode)
    {
        if (!IsValidCode(accessCode))
            return Unauthorized("Invalid access code");

        _service.Delete(id);
        return Ok("Deleted");
    }
}