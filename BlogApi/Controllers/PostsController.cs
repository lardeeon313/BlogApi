using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers
{
    public class PostsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
