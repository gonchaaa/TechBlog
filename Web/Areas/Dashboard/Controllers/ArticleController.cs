using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Web.Data;
using Web.Models;

namespace Web.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize]
    public class ArticleController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public ArticleController(AppDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            var taglist=_context.Tags.ToList();
            ViewBag.Tags = new SelectList(taglist,"Id","TagName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Article article, List<int> tagIds)
        {

            try
            {
                article.UserId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value ;
                article.SeoUurl = "senanmuellim";
                article.CreatedDate = DateTime.Now;
                article.UpdatedDate = DateTime.Now;
                await  _context.Articles.AddAsync(article);
               await _context.SaveChangesAsync();
                for(int i = 0; i < tagIds.Count; i++)
                {
                    ArticleTag articletag = new()
                    {
                        TagId = tagIds[i],
                        ArticleId = article.Id
                    };
                    await _context.ArticleTags.AddAsync(articletag);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

            }
            return View();
        }
    }
}
