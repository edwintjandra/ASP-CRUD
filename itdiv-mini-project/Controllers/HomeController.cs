using Microsoft.AspNetCore.Mvc;

namespace itdiv_mini_project.Controllers
{
    public class HomeController : Controller
    {
        //return View("CustomEditView", product);

        public IActionResult Index()
        {
            return View("Index");
        }

        public IActionResult About()
        {
            return View("About");
        }
    }
}
