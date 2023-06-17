using Web.Models;

namespace Web.ViewModels
{
    public class DetailVM
    {
        public Article Article { get; set; }
        public Article PrevArticle { get; set; }
        public Article NextArticle { get; set; }
        public List<Article> Similar { get; set; }
        public List<Article> TopArticles { get; set;}
        public List<Comments> Comments { get; set;}
        public List<Comments> RecentReviews { get; set;}
    }
}
