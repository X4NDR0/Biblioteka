namespace Biblioteka.Facades.Sql.Models
{
    public class Book
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int ReleaseYear { get; set; }
        public Member Member { get; set; }
    }
}
