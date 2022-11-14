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
        public async Task<IActionResult> GetAllGames() 
        {
            var games = data.Games.ToList();
            

            List<GetGameResponse> responses = new List<GetGameResponse>();

            foreach (var game in games)
            {
                var gameResponse = new GetGameResponse();
                responses.Add(gameResponse);

                gameResponse.GameId = game.Id;
                gameResponse.GameName = game.Name;
                gameResponse.Price = game.Price;
                gameResponse.PriceCurrency = game.PriceCurrency;
                gameResponse.GenreId = game.GenreId;
                gameResponse.Genre = data.Genres.Where(g => g.Id == game.GenreId).First().Name;
                gameResponse.Tags = new List<TagResponse>();

                var tags = data.Tags.Where(t => t.Games.Contains(game)).ToList();
                foreach (var tag in tags)
                {
                    var tagResponse = new TagResponse();

                    tagResponse.Id = tag.Id;
                    tagResponse.Name = tag.Name;

                    gameResponse.Tags.Add(tagResponse);
                }

            }
            return Ok(responses);
        }

        [HttpGet, Route("GetGameById")]
        public async Task<IActionResult> GetGameById([FromBody] GetGameRequest request) 
        {
            Game game = data.Games.First(x => x.Id == request.GameId);

            var gameResponse = new GetGameResponse();
           

            gameResponse.GameId = game.Id;
            gameResponse.GameName = game.Name;
            gameResponse.Price = game.Price;
            gameResponse.PriceCurrency = game.PriceCurrency;
            gameResponse.GenreId = game.GenreId;
            gameResponse.Genre = data.Genres.Where(g => g.Id == game.GenreId).First().Name;
            gameResponse.Tags = new List<TagResponse>();

            var tags = data.Tags.Where(t => t.Games.Contains(game)).ToList();
            foreach (var tag in tags)
            {
                var tagResponse = new TagResponse();

                tagResponse.Id = tag.Id;
                tagResponse.Name = tag.Name;

                gameResponse.Tags.Add(tagResponse);
            }


            return Ok(gameResponse);
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

    public class TagResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class GetGameResponse
    {
        public int GameId { get; set; }
        public string GameName { get; set; }
        public decimal Price { get; set; }
        public string PriceCurrency { get; set; }
        public int GenreId { get; set; }
        public string Genre { get; set; }
        public List<TagResponse> Tags { get; set; }
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
