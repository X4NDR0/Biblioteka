namespace Biblioteka.Models
{
    public class Book
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public int ReleaseYear { get; set; }
        public Member Member { get; set; }
    }
}
