using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using API_Classes;
using WEB_API_Business_Tier.Models;

namespace WEB_API_Business_Tier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly string DataApiUrl = "http://localhost:5009/api/transaction";

        // Deposit function
        [HttpPost("deposit")]
        public IActionResult Deposit([FromQuery] uint accountNo, [FromQuery] decimal amount)
        {
            var client = new RestClient(DataApiUrl);
            var finalUrl = $"deposit?accountNo={accountNo}&amount={amount}";
            Console.WriteLine("Constructed URL: " + DataApiUrl + "/" + finalUrl);
            var request = new RestRequest(finalUrl, Method.Post);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Content);
            }
            else
            {
                return BadRequest($"Error: {response.Content}");

            }
        }

        // Withdraw function
        [HttpPost("withdraw")]
        public IActionResult Withdraw(uint accountNo, decimal amount)
        {
            var client = new RestClient(DataApiUrl);
            var request = new RestRequest($"withdraw?accountNo={accountNo}&amount={amount}", Method.Post);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Content);
            }
            else
            {
                return BadRequest(response.ErrorMessage);
            }
        }

        // Get all transactions
        [HttpGet("all")]
        public IActionResult GetAllTransactions()
        {
            var client = new RestClient(DataApiUrl);
            var request = new RestRequest("all", Method.Get);

            var response = client.Execute<List<TransactionIntermed>>(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Data);
            }
            else
            {
                return NotFound(response.ErrorMessage);
            }
        }

        // Get transactions by accountNo
        [HttpGet("{accountNo}")]
        public IActionResult GetTransactionsByAccountNo(uint accountNo)
        {
            var client = new RestClient(DataApiUrl);
            var request = new RestRequest($"{accountNo}", Method.Get);

            var response = client.Execute<List<TransactionIntermed>>(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Data);
            }
            else
            {
                return NotFound(response.ErrorMessage);
            }
        }

        [HttpPost("transfer")]
        public IActionResult Transfer(TransferInputModel transferData)
        {
            var client = new RestClient(DataApiUrl);
            var request = new RestRequest("transfer", Method.Post);

            // Set the Content-Type header to application/json
            request.AddHeader("Content-Type", "application/json");

            // Serialize the transferData object to JSON and add it to the request body
            request.AddJsonBody(transferData);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Content);
            }
            else
            {
                return BadRequest($"Error: {response.Content}");
            }
        }



    }
}
