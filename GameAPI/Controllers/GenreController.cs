using Data;
using GameAPI.Services;
using Mapping;
using Microsoft.AspNetCore.Mvc;

namespace GameAPI.Controllers
{
    [Route("api/Genre")]
    [ApiController]
    public class GenreController : Controller
    {
       
        private GenreService service;
        public GenreController( GenreService _service)
        {
            this.service = _service;
        }

        [HttpPost, Route("AddGenre")]
        public async Task<IActionResult> AddGenre([FromBody] AddGenreRequest request)
        {
            await service.AddGenre(request.Name);

            return Ok();
        }
    }

    public class AddGenreRequest
    {
        public string Name { get; set; }
    }
}
