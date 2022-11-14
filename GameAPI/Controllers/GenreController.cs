using Data;
using Mapping;
using Microsoft.AspNetCore.Mvc;

namespace GameAPI.Controllers
{
    [Route("api/Genre")]
    [ApiController]
    public class GenreController : Controller
    {
        private ApplicationDbContext _data;
        public GenreController(ApplicationDbContext data)
        {
            this._data = data;
        }

        [HttpPost, Route("AddGenre")]
        public async Task<IActionResult> AddGenre([FromBody] AddGenreRequest request)
        {
            var newGenre = new Genre
            {
                Name = request.Name,
            };

            await _data.Genres.AddAsync(newGenre);
            await _data.SaveChangesAsync();

            return Ok();
        }
    }

    public class AddGenreRequest
    {
        public string Name { get; set; }
    }
}
