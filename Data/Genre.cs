namespace Data
{
    public class Genre
    {
        public Genre()
        {
            Games = new List<Game>();
        }

        public int Id { get; set; }

        public string Name { get; set; }
        public List<Game> Games { get; set; }
    }
}
