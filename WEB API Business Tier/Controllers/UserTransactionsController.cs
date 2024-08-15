using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using API_Classes;

namespace WEB_API_Business_Tier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTransactionsController : Controller
    {
        private readonly TransactionController _transactionController = new TransactionController();

        [HttpGet("view")]
        public IActionResult GetUserTransactionsView()
        {
            var allTransactionsResponse = _transactionController.GetAllTransactions();
            if (allTransactionsResponse is OkObjectResult okResult)
            {
                var allTransactions = okResult.Value as List<TransactionIntermed>;
                return View("UserTransactionsView", allTransactions);
            }
            else
            {
                // Handle the error appropriately. Maybe return a view with an error message.
                ViewBag.Error = "Error fetching transactions.";
                return View("ErrorView");
            }
        }
    }
}
