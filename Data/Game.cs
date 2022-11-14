namespace Data
{
    public class Game
    {
        public Game()
        {
            Tags = new List<Tag>();
        }

        
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Currency { get; set; }

        public int GenreId { get; set; }

        public Genre Genre { get; set; }

        public List<Tag> Tags { get; set; }

        // can have one genre can have many tags tags can have many games 
    }
}