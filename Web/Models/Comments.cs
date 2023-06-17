namespace Web.Models
{
    public class Comments
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public virtual string UserId { get; set; }
        public User User { get; set; }
    }
}
