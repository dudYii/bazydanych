namespace projekt_bd_wypozyczalnia_gier.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string NameAndLastName { get; set; } 
        public string Email { get; set; } 
        public string PhoneNumber { get; set; } 
        public string Address { get; set; } 

        public ICollection<Rental> Rentals { get; set; }
    }
}