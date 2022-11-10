using Data;
using Mapping;
using Microsoft.AspNetCore.Mvc;

namespace GameAPI.Controllers
{
    [Route("api/Tag")]
    [ApiController]
    public class TagController : Controller
    {
        private ApplicationDbContext data;
        public TagController(ApplicationDbContext _data)
        {
            data = _data;
        }

        [HttpPost, Route("AddTag")]
        public async Task<IActionResult> AddTag([FromBody] AddTagRequest request)
        {
            var newTag = new Tag
            {
                Name = request.Name
            };

            await data.Tags.AddAsync(newTag);
            await data.SaveChangesAsync();
            return Ok();
        
        }
    }

    public class AddTagRequest
    {
        public string Name { get; set; }
    }
}
