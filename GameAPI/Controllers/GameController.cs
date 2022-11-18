using Data;
using GameAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GameAPI.Controllers
{
    [Route("api/Game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private GameService service;

        public GameController(GameService _service)
        {
            this.service = _service;
        }

        [HttpPost, Route("AddGame")]
        public async Task<IActionResult> AddGameAsync([FromBody] AddGameRequest request)
        {
            service.AddGame(request.Name, request.Price, request.GenreId, request.Tags);
            return Ok();
        }

        [HttpGet, Route("GetGames")]
        public async Task<IActionResult> GetAllGames() 
        {
            List<GetGameResponse> responses = await service.GetGames();
            return Ok(responses);
        }

        [HttpGet, Route("GetGameById")]
        public async Task<IActionResult> GetGameById([FromQuery] GetGameRequest request) 
        {
            GetGameResponse gameResponse = await service.GetGameById();

            if (gameResponse is null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(gameResponse);
            }
           
        }

        [HttpPost, Route("SetPrice")]
        public async Task<IActionResult> SetPriceAsync([FromBody] SetPriceRequest request)
        {
            await service.SetPrice((int)request.GameId, request.Price);
            return Ok();
        }
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

        public Money Price { get; set; }

        public int GenreId { get; set; }

        public List<Tag> Tags { get; set; }
    }

    public class SetPriceRequest
    {
        [Required]
        public int? GameId { get; set; }

    
        public Money Price { get; set; }
    }

    //public class GamePrice
    //{
    //    [Required]
    //    public decimal Amount { get; set; }

    //    [Required]
    //    public string Currency { get; set; }
    //}
}
