using Data;
using Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace GameAPI.Controllers
{
    [Route("api/Game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private ApplicationDbContext _data;

        public GameController(ApplicationDbContext data)
        {
             this._data = data;
        }

        [HttpPost, Route("AddGame")]
        public async Task<IActionResult> AddGameAsync([FromBody] AddGameRequest request)
        {
            var newGame = new Game
            {
                Name = request.Name,
                Price = request.Price,
                GenreId = request.GenreId,
                Tags = request.Tags
            };

            await _data.Games.AddAsync(newGame).ConfigureAwait(false);
            await _data.SaveChangesAsync().ConfigureAwait(false);

            return Ok();

        }

        [HttpGet, Route("GetGames")]
        public async Task<IActionResult> GetAllGames() 
        {
            List<Game> games = _data.Games.ToList();
            
            List<GetGameResponse> responses = new List<GetGameResponse>();

            foreach (var game in games)
            {
                var gameResponse = new GetGameResponse();
                responses.Add(gameResponse);

                gameResponse.GameId = game.Id;
                gameResponse.GameName = game.Name;
                gameResponse.Price = game.Price;
                gameResponse.PriceCurrency = game.Currency;
                gameResponse.GenreId = game.GenreId;
                gameResponse.Genre = _data.Genres.Where(g => g.Id == game.GenreId).First().Name;
                gameResponse.Tags = new List<TagResponse>();

                var tags = _data.Tags.Where(t => t.Games.Contains(game)).ToList();
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
        public async Task<IActionResult> GetGameById([FromQuery] GetGameRequest request) 
        {
            var game = await (from theGame in _data.Games.Include(x=>x.Genre).Include(x=>x.Tags)
                             where theGame.Id == 2
                             select theGame)
                            .SingleOrDefaultAsync().ConfigureAwait(false);

            if (game is null)
                return Ok();

            var gameResponse = new GetGameResponse();
            gameResponse.GameId = game.Id;
            gameResponse.GameName = game.Name;
            gameResponse.Price = game.Price;
            gameResponse.PriceCurrency = game.Currency;
            gameResponse.GenreId = game.GenreId;
            gameResponse.Genre = game.Genre.Name;
            gameResponse.Tags = new List<TagResponse>();

            foreach (Tag tag in game.Tags)
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

            IEnumerable<string> currencySymbols = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Select(x => (new RegionInfo(x.LCID)).ISOCurrencySymbol)
                .Distinct()
                .OrderBy(x => x);

            if (!currencySymbols.Any(x => x == request.Price.Currency))
            {
                return BadRequest();
            }


            Game game = _data.Games.Where(x => x.Id == request.GameId).SingleOrDefault();
            if (game is null)
                return BadRequest();

            game.Price = request.Price.Amount;
            game.Currency = request.Price.Currency;

            await _data.SaveChangesAsync().ConfigureAwait(false);

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
        public GetGameResponse()
        {
            Tags = new List<TagResponse>();
        }

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
        public AddGameRequest()
        {
            Tags = new List<Tag>();
        }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int GenreId { get; set; }

        public List<Tag> Tags { get; set; }
    }

    public class SetPriceRequest
    {
        [Required]
        public int? GameId { get; set; }

    
        public GamePrice Price { get; set; }
    }

    public class GamePrice
    {
        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; }
    }
}
