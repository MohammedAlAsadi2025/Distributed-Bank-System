using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using Newtonsoft.Json;
using API_Classes;

namespace WEB_API_Business_Tier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly string DataApiUrl = "http://localhost:5009/api/accounts";
        [HttpGet("count")]
        public IActionResult GetNumEntries()
        {
            var client = new RestClient(DataApiUrl);
            var request = new RestRequest("count", Method.Get);
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Content);
            }
            else
            {
                return StatusCode((int)response.StatusCode, response.ErrorMessage);
            }
        }

        [HttpPost]
        public IActionResult CreateAccount([FromBody] DataIntermed newAccount)
        {
            var client = new RestClient(DataApiUrl);
            var request = new RestRequest("", Method.Post);
            request.AddJsonBody(newAccount);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return Ok("Successfully inserted");
            }
            else
            {
                // Forward the error message from the Data Tier to the client
                return BadRequest(response.Content);
            }
        }

        [HttpPut("{accountNo}")]
        public IActionResult UpdateAccount(uint accountNo, [FromBody] DataIntermed accountData)
        {
            var client = new RestClient(DataApiUrl);
            var request = new RestRequest($"{accountNo}", Method.Put);
            request.AddJsonBody(accountData);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Content);
            }
            else
            {
                return StatusCode((int)response.StatusCode, response.ErrorMessage);
            }
        }

        [HttpDelete("{accountNo}")]
        public IActionResult DeleteAccount(uint accountNo)
        {
            var client = new RestClient(DataApiUrl);
            var request = new RestRequest($"{accountNo}", Method.Delete);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return Ok("Successfully deleted");
            }
            else
            {
                return StatusCode((int)response.StatusCode, response.ErrorMessage);
            }
        }
    }
}
