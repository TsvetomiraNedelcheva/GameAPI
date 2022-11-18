using Data;
using Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameAPI.Services
{
    public class GameService
    {

        private ApplicationDbContext _data;

        public GameService(ApplicationDbContext data)
        {
            this._data = data;
        }

        public async void AddGame(string name, Money price, int genreId, List<Tag> tags)
        {
            Game newGame = new Game(name, price, genreId, tags);

            await _data.Games.AddAsync(newGame).ConfigureAwait(false);
            await _data.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<List<GetGameResponse>> GetGames()
        {
            List<Game> games = _data.Games.ToList();

            List<GetGameResponse> responses = new List<GetGameResponse>();

            foreach (var game in games)
            {
                var gameResponse = new GetGameResponse(); //only responses can be in services/ separate class
                responses.Add(gameResponse);

                gameResponse.GameId = game.Id;
                gameResponse.GameName = game.Name;
                gameResponse.Price = game.Price;
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

            return responses;
        }

        public async Task<GetGameResponse> GetGameById()
        {
            var game = await (from theGame in _data.Games.Include(x => x.Genre).Include(x => x.Tags)
                              where theGame.Id == 2
                              select theGame)
                           .SingleOrDefaultAsync().ConfigureAwait(false);

            //if (game is null)
            //    return Ok();

            var gameResponse = new GetGameResponse();
            gameResponse.GameId = game.Id;
            gameResponse.GameName = game.Name;
            gameResponse.Price = game.Price;
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

            return gameResponse;
        }

        public async Task SetPrice(int gameId, Money price)
        {
            Game game = _data.Games.Where(x => x.Id == gameId).SingleOrDefault();

            //if (game is null)
            //    return BadRequest();

            game.SetPrice(game, price);

            await _data.SaveChangesAsync().ConfigureAwait(false);

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

        public Money Price { get; set; }

        public int GenreId { get; set; }

        public string Genre { get; set; }

        public List<TagResponse> Tags { get; set; }
    }
}
