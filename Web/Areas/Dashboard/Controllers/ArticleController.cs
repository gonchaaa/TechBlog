using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Web.Data;
using Web.Helpers;
using Web.Models;

namespace Web.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize]
    public class ArticleController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ArticleController(AppDbContext context, IHttpContextAccessor contextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var articles = _context.Articles.Include(x => x.User).Include(y => y.ArticleTags).ThenInclude(z => z.Tag).ToList();
            return View(articles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var taglist = _context.Tags.ToList();
            ViewBag.Tags = new SelectList(taglist, "Id", "TagName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Article article, List<int> tagIds,IFormFile newPhoto)
        {

            try
            {
                //  /uploads/jhbvhbvdhvxyudvuy.jpg
                string path = "/uploads/" + Guid.NewGuid() + Path.GetExtension(newPhoto.FileName) ;
               //garbage collector
                using(var filestream=new FileStream(_webHostEnvironment.WebRootPath+path,FileMode.Create))
                {
                    newPhoto.CopyTo(filestream);
                }
                article.PhotoUrl = path;
                article.UserId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                article.SeoUurl = SeoHelper.SeourlCreater(article.Title);
                article.CreatedDate = DateTime.Now;
                article.UpdatedDate = DateTime.Now;
                await _context.Articles.AddAsync(article);
                await _context.SaveChangesAsync();
                for (int i = 0; i < tagIds.Count; i++)
                {
                    ArticleTag articletag = new()
                    {
                        TagId = tagIds[i],
                        ArticleId = article.Id
                    };
                    await _context.ArticleTags.AddAsync(articletag);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                return View();
            }
        }


        public IActionResult Edit(int id)
        {
            var article = _context.Articles.Include(x=>x.ArticleTags).SingleOrDefault(a => a.Id == id);
            if(article == null || id==null)
            {
                return NotFound();
            }
            var tags=_context.Tags.ToList();
            ViewData["taglist"] = tags;
            return View(article);
        }
        public IActionResult Edit(Article article,List<int> tagIds,IFormFile newPhoto)
        {


            article.UserId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            article.SeoUurl = SeoHelper.SeourlCreater(article.Title);
            article.UpdatedDate= DateTime.Now;  
            _context.Articles.Update(article);
            _context.SaveChanges();
            var articleTag = _context.ArticleTags.Where(a=>a.ArticleId==article.Id).ToList();
            _context.ArticleTags.RemoveRange(articleTag);
            _context.SaveChanges();

            for (int i = 0; i < tagIds.Count; i++)
            {
                ArticleTag articletag = new()
                {
                    TagId = tagIds[i],
                    ArticleId = article.Id
                };
                 _context.ArticleTags.AddAsync(articletag);
            }
             _context.SaveChangesAsync();

            _context.SaveChanges();

            return RedirectToAction("Index");

        }

    }
}
