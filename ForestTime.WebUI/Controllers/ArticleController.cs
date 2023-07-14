using ForestTime.WebUI.Data;
using ForestTime.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForestTime.WebUI.Controllers
{
    public class ArticleController : Controller
    {
        private readonly AppDbContext _context;

        public ArticleController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            return View();
        }
        
        public IActionResult Detail(int id)
        {
            try
            {
            var article=_context.Articles.Include(x=>x.User).Include(x=>x.Category).Include(x=>x.ArticleTags).ThenInclude(x=>x.Tag).FirstOrDefault(x=>x.Id == id);
                var topArticles = _context.Articles.OrderByDescending(x=>x.Views).Take(3).ToList();
                var recentArticles=_context.Articles.OrderByDescending(x=>x.Id).Take(3).ToList();
               if(article == null)
            {
                return NotFound();
            }
                DetailVM vm = new()
                {
                    Article = article,
                    TopArticles = topArticles,
                    RecentAddedArticles= recentArticles
            };
                article.Views += 1;
                _context.Articles.Update(article);
                _context.SaveChanges();
            return View(vm);
            }
            catch (Exception)
            {

                return NotFound();
            }
        }
    }
}
