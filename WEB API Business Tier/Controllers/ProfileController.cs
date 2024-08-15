using Microsoft.AspNetCore.Mvc;
using API_Classes;
using Newtonsoft.Json;

namespace WEB_API_Business_Tier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : Controller
    {
        [HttpGet("view")]
        public IActionResult GetView()
        {
            if (Request.Cookies.ContainsKey("SessionID"))
            {
                var sessionCookieValue = Request.Cookies["SessionID"];
                if (sessionCookieValue == "1234567")
                {
                    // Fetch the UserName from the cookies.
                    var userName = Request.Cookies["UserName"];

                    // Use the UserProfileController to get the user details.
                    var userProfileController = new UserProfileController();
                    var userProfileResponse = userProfileController.GetUserProfile(userName);

                    // Check if the response was successful and the user was found.
                    if (userProfileResponse is OkObjectResult okResult)
                    {
                        var user = JsonConvert.DeserializeObject<API_Classes.UserProfileIntermed>(okResult.Value.ToString());
                        return PartialView("ProfileViewAuthenticated", user);
                    }
                    else
                    {
                        // Handle the case where the user data was not found or there was an error.
                        // This logic can be adjusted as needed.
                        ViewBag.Error = "Error fetching user data.";
                        return PartialView("ProfileViewDefault");
                    }
                }
            }
            return PartialView("ProfileViewDefault");
        }
    }
}
