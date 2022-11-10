namespace Data
{
    public class Tag
    {
        public Tag()
        {
            Games = new List<Game>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public List<Game> Games { get; set; }
    }
}
