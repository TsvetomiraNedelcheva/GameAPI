namespace Data
{
    public class Game
    {
        Game()
        {
            Tags = new List<Tag>();
        }

        public Game(string name, Money price, int genreId, List<Tag> tags) : base()
        {
            Name = name;
            Price = price;
            GenreId = genreId;
            Tags = tags;
        }


        public int Id { get; set; }

        public string Name { get; set; }

        public Money Price { get; set; }

        public int GenreId { get; set; }

        public Genre Genre { get; set; }

        public List<Tag> Tags { get; set; }

        public void SetPrice(Game game, Money price)
        {
            game.Price.Amount = price.Amount;
            game.Price.Currency = price.Currency;
        }

        //method add tag?

        // can have one genre can have many tags tags can have many games 
    }
}