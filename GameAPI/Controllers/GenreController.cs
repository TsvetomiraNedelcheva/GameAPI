﻿using Data;
using Mapping;
using Microsoft.AspNetCore.Mvc;

namespace GameAPI.Controllers
{
    [Route("api/Genre")]
    [ApiController]
    public class GenreController : Controller
    {
        private ApplicationDbContext data;
        public GenreController(ApplicationDbContext _data)
        {
            data = _data;
        }

        [HttpPost, Route("AddGenre")]
        public async Task<IActionResult> AddGenre([FromBody] AddGenreRequest request)
        {
            var newGenre = new Genre
            {
                Name = request.Name,
            };

            await data.Genres.AddAsync(newGenre);
            await data.SaveChangesAsync();
            return Ok();
        }
    }

    public class AddGenreRequest
    {
        public string Name { get; set; }
    }
}
