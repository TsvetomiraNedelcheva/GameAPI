using Data;
using Mapping;

namespace GameAPI.Services
{
    public class GenreService
    {
        private ApplicationDbContext _data;
        public GenreService(ApplicationDbContext data)
        {
            this._data = data;
        }

        public async Task AddGenre(string name)
        {
            var newGenre = new Genre
            {
                Name = name,
            };

            await _data.Genres.AddAsync(newGenre);
            await _data.SaveChangesAsync();

        }
    }
}
