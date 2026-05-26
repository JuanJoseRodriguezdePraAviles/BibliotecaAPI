namespace BibliotecaAPI.Models
{
    public class Lending
    {
        public int ID { get; set; }

        public int BookID { get; set; }
        public Book Book { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }

        public DateTime LendingDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }

    }
}
