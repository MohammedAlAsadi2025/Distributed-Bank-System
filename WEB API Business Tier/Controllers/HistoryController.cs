using API_Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WEB_API_Business_Tier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : Controller
    {
        [HttpGet("view")]
        public IActionResult GetHistory()
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

                        // Now, use the TransactionController to get the transaction history using accountNo.
                        var transactionController = new TransactionController();
                        var transactionsResponse = transactionController.GetTransactionsByAccountNo(accountNo);

                        // Check if the response was successful and transactions were found.
                        if (transactionsResponse is OkObjectResult transactionsOkResult && transactionsOkResult.Value is List<TransactionIntermed> transactions && transactions.Count > 0)
                        {
                            return PartialView("HistoryViewAuthenticated", transactions);
                        }
                        else
                        {
                            // Handle the case where the transactions data was not found or there was an error.
                            ViewBag.Error = "Error fetching transaction history.";
                            return PartialView("HistoryViewDefault");
                        }
                    }
                    else
                    {
                        // Handle the case where the user profile data was not found or there was an error.
                        ViewBag.Error = "Error fetching user profile data.";
                        return PartialView("HistoryViewDefault");
                    }
                }
            }
            return PartialView("HistoryViewDefault");
        }
    }


}
