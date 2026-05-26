namespace BibliotecaAPI.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string DNI { get; set; }
        public DateTime RegistrationDate { get; set; }
        public ICollection<Lending>? Lendings { get; set; }
    }
}
