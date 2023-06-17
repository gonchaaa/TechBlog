using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Web.Data;
using Web.Models;
using Web.ViewModels;

namespace Web.Controllers
{
    public class ArticleController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public ArticleController(AppDbContext context,  IHttpContextAccessor httpContext)
        {
            _context = context;
         
            _httpContext = httpContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddComment(Comments articlecomment,int articleId)
        {

            try
            {
                articlecomment.ArticleId = articleId;
                articlecomment.UserId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
               
                
                _context.Comments.Add(articlecomment);
                _context.SaveChanges();
            return RedirectToAction("Detail", "Article", new {Id=articleId});
            }
            catch (Exception)
            {

                return RedirectToAction("Index");

            }
        }
        public IActionResult Detail(int id)
        {
            try
            {
                var article = _context.Articles.Include(x=>x.User).Include(x=>x.ArticleTags).ThenInclude(x=>x.Tag).SingleOrDefault(x => x.Id == id);
                var topArticles = _context.Articles.OrderByDescending(z=>z.Views).Take(3).ToList();
                var nextArticle=_context.Articles.FirstOrDefault(x=>x.Id > id);
                var prevArticle = _context.Articles.FirstOrDefault(x => x.Id < id);
                var similar = _context.Articles.Where(x => x.ArticleTags.Select(x => x.TagId).Contains(article.ArticleTags[0].TagId)&& x.Id!=id).ToList();
                var recentReviews = _context.Comments.Include(x=>x.Article).OrderByDescending(x=>x.Id).GroupBy(x=>x.ArticleId).Select(y=>y.First()).Take(3).ToList();


                if (article == null)
                {
                    return NotFound();
                }
                var articleComments = _context.Comments.Include(x=>x.User).Where(x=>x.ArticleId==id).ToList();
                DetailVM vm = new()
                {
                    Article = article,
                    TopArticles = topArticles,
                    Comments = articleComments,
                    PrevArticle=prevArticle,
                    NextArticle=nextArticle,
                    Similar=similar,
                    RecentReviews=recentReviews
                };
                article.Views += 1;
                _context.Articles.Update(article);
                _context.SaveChanges();


                return View(vm);
            }
            catch (Exception ex)
            {

                return NotFound();

            }
        }
    }
}
