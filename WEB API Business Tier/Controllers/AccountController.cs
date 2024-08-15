using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WEB_API_Business_Tier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
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

                    if (userProfileResponse is OkObjectResult userProfileOkResult)
                    {
                        var user = JsonConvert.DeserializeObject<API_Classes.UserProfileIntermed>(userProfileOkResult.Value.ToString());
                        uint accountNo = user.AccountNo;

                        // Now, use the AccountNoController to get the account details using accountNo.
                        var accountDataController = new AccountNoController();
                        var accountResponse = accountDataController.GetAccountDetails(accountNo);

                        // Check if the response was successful and the account was found.
                        if (accountResponse is OkObjectResult accountOkResult)
                        {
                            Console.WriteLine(accountOkResult.Value.ToString());
                            var account = accountOkResult.Value as API_Classes.DataIntermed;
                            return PartialView("AccountViewAuthenticated", account);
                        }
                        else
                        {
                            // Handle the case where the account data was not found or there was an error.
                            ViewBag.Error = "Error fetching account data.";
                            return PartialView("AccountViewDefault");
                        }
                    }
                    else
                    {
                        // Handle the case where the user profile data was not found or there was an error.
                        ViewBag.Error = "Error fetching user profile data.";
                        return PartialView("AccountViewDefault");
                    }
                }
            }
            return PartialView("AccountViewDefault");
        }
    }
}
