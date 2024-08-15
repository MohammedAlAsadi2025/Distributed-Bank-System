using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using API_Classes;

namespace WEB_API_Business_Tier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferController : Controller
    {

        [HttpGet("view")]
        public IActionResult GetTransactionView()
        {
            // Your logic to return the view goes here.
            return PartialView("TransferViewAuthenticated");
        }

        [HttpPost("ProcessTransfer")]
        public IActionResult ProcessTransfer(TransferInputModel input)
        {
            // Validate the input
            if (!ModelState.IsValid)
            {
                return View("TransferViewAuthenticated", input); // Return with validation errors
            }

            // Get the logged-in user's account number using a similar approach to the AccountController
            uint senderAccountNo;
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
                        senderAccountNo = user.AccountNo;
                    }
                    else
                    {
                        // Handle the case where the user profile data was not found or there was an error.
                        ViewBag.Error = "Error fetching user profile data.";
                        return View("TransferViewAuthenticated", input);
                    }
                }
                else
                {
                    // If the session is not valid
                    ViewBag.Error = "Invalid session.";
                    return View("TransferViewAuthenticated", input);
                }
            }
            else
            {
                // If the SessionID cookie is not found
                ViewBag.Error = "Please log in to continue.";
                return View("TransferViewAuthenticated", input);
            }

            // Prepare the data to send to the Data Tier
            var transferData = new TransferInputModel
            {
                SenderAccountNo = senderAccountNo,
                RecipientAccountNo = input.RecipientAccountNo,
                Amount = input.Amount,
                Description = input.Description
            };

            // Call the Transfer method from the TransactionController
            var transactionController = new TransactionController();
            var transferResult = transactionController.Transfer(transferData);

            // Check if the transfer was successful
            if (transferResult is OkObjectResult)
            {
                return RedirectToAction("SuccessPage"); // Or wherever you want to redirect after a successful transfer
            }
            else
            {
                // Handle the error case, maybe return to the form with an error message
                ViewBag.Error = "Transfer failed!";
                return View("TransferViewAuthenticated", input);
            }
        }




    }
}
