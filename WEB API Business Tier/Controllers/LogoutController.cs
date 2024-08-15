using Microsoft.AspNetCore.Mvc;

namespace WEB_API_Business_Tier.Controllers
{
    [Route("api/[controller]")]
    public class LogoutController : Controller
    {
        [HttpGet]
        public IActionResult GetView()
        {
            Response.Cookies.Delete("UserName");
            Response.Cookies.Delete("IsAdmin");
            Response.Cookies.Delete("SessionID");
            return PartialView("LogoutView");
        }
    }
}

