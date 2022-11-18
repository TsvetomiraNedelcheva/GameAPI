using Data;
using Mapping;

namespace GameAPI.Services
{
    public class TagService
    {
        private ApplicationDbContext data;
        public TagService(ApplicationDbContext _data)
        {
            data = _data;
        }

        public async Task AddTag(string name)
        {
            var newTag = new Tag
            {
                Name = name
            };

            await data.Tags.AddAsync(newTag);
            await data.SaveChangesAsync();
        }
    }
}
