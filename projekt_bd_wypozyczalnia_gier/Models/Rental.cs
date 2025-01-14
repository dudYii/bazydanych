namespace projekt_bd_wypozyczalnia_gier.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int CustomerId { get; set; } 
        public Customer Customer { get; set; }

    }
}