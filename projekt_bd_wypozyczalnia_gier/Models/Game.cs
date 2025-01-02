namespace projekt_bd_wypozyczalnia_gier.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Platform { get; set; } 
        public bool Available { get; set; } 

        public ICollection<RentalDetails> RentalDetails { get; set; }
    }
}