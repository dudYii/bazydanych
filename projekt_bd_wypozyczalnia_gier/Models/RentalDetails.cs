namespace projekt_bd_wypozyczalnia_gier.Models
{
    public class RentalDetails
    {
        public int Id { get; set; }
        public int RentalId { get; set; } 
        public Rental Rental { get; set; } 

        public int GameId { get; set; } 
        public Game Game { get; set; } 

        public int Quantity { get; set; } 
        public DateTime RentalDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal Price { get; set; }
    }
}