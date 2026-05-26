namespace BibliotecaAPI.Models
{
    public class Book
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Autor { get; set; }
        public string Category { get; set; }
        public int Year { get; set; }
        public Boolean Available { get; set; }
        public ICollection<Lending>? Lendings { get; set; }
    }
}
