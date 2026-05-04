using Microsoft.AspNetCore.Mvc;
using Maintainancebackend.Models;

namespace Maintainancebackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _service;

        public UsersController(UserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int buildingId)
        {
            return Ok(await _service.GetAllAsync(buildingId));
        }
    }
}