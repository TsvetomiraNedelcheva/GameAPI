using Data;
using GameAPI.Services;
using Mapping;
using Microsoft.AspNetCore.Mvc;

namespace GameAPI.Controllers
{
    [Route("api/Tag")]
    [ApiController]
    public class TagController : Controller
    {
        private TagService service;
        public TagController(TagService _service)
        {
            service = _service;
        }

        [HttpPost, Route("AddTag")]
        public async Task<IActionResult> AddTag([FromBody] AddTagRequest request)
        {
            service.AddTag(request.Name);
            return Ok();
        
        }
    }

    public class AddTagRequest
    {
        public string Name { get; set; }
    }
}
