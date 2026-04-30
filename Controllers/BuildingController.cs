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
    public IActionResult Create(Building b)
    {
        _service.Create(b);
        return Ok("Created");
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Building b)
    {
        _service.Update(id, b);
        return Ok("Updated");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _service.Delete(id);
        return Ok("Deleted");
    }
}