using Web.Models;

namespace Web.ViewModels
{
    public class HomeVM
    {
        public List<Article> HomeArticle { get; set; }
        public List<Tag> HomeTags { get; set; }

    }
}
