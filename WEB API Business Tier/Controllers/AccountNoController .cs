using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using API_Classes;

namespace WEB_API_Business_Tier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountNoController : ControllerBase
    {
        private readonly string DataApiUrl = "http://localhost:5009/api/accounts";

        [HttpGet("{accountNo}")]
        public IActionResult GetAccountDetails(uint accountNo)
        {
            var client = new RestClient(DataApiUrl);
            var request = new RestRequest(accountNo.ToString(), Method.Get);
            var response = client.Execute<DataIntermed>(request);

            // Log the raw content of the response
            Console.WriteLine($"Response from Data Tier: {response.Content}");

            if (response.IsSuccessful)
            {
                return Ok(response.Data); // Return the account details from the Data Tier
            }
            else
            {
                Console.WriteLine($"Error from Data Tier: {response.ErrorMessage}");
                return StatusCode((int)response.StatusCode, response.ErrorMessage);
            }
        }
    }
}
