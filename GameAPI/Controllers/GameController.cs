using Data;
using Mapping;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet, Route("GetGames")]
        public async Task<IActionResult> GetGames() //return response model like request model so that the response lists look better in postman
        {
            var games = data.Games.ToList();
            return Ok(games);
        }

        [HttpGet, Route("GetGame")]
        public async Task<IActionResult> GetGame([FromBody] GetGameRequest request) //return response model like request model so that the response looks better in postman
        {
            Game game = data.Games.First(x => x.Id == request.GameId);
            return Ok(game);
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

    public class GetGameRequest
    {
        public int GameId { get; set; }
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
