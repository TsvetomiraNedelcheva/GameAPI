using Data;
using Mapping;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GameAPI.Controllers
{
    [Route("api/Game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private ApplicationDbContext data;
        public GameController(ApplicationDbContext _data)
        {
             data = _data;
        }

        [HttpPost, Route("AddGame")]
        public async Task<IActionResult> AddGame([FromBody] AddGameRequest request)
        {
            var newGame = new Game
            {
                Name = request.Name,
                Price = request.Price,
                GenreId = request.GenreId,
                Tags = request.Tags
            };

            await data.Games.AddAsync(newGame);
            await data.SaveChangesAsync();
            return Ok();

        }

        [HttpGet]
        public IActionResult GetGames() 
        {
            List<string> games = new List<string>() { "SomeGame", "GameName", "Test" };

            if (games.Where(x => x.Equals("Super Mario")).Any() == false)
                return NotFound();

            return Accepted(games);
        }

        [HttpPost, Route("SetPrice")]
        public async Task<IActionResult> SetPriceAsync([FromBody] SetPriceRequest request)
        {
            if (!data.Games.Any(x => x.Id == request.GameId))
            {
                return BadRequest();
            }

            Game game = data.Games.First(x => x.Id == request.GameId);
            game.Price = request.Price.Amount;
            string priceCurrency = request.Price.Amount + request.Price.Currency;
            game.PriceCurrency = priceCurrency;
            await data.SaveChangesAsync();
            return Ok();
        }
    }

    public class AddGameRequest
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int GenreId { get; set; }
        public List<Tag> Tags { get; set; }
    }

    public class SetPriceRequest
    {
        public int GameId { get; set; }

        public GamePrice Price { get; set; }
    }

    public class GamePrice
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
