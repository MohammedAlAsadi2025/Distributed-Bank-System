using API_Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace WEB_API_Business_Tier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageController : Controller
    {
        [HttpGet("view")]
        public IActionResult ManageView()
        {
            return View("ManageView");
        }


        [HttpGet("searchByName")]
        public IActionResult SearchByName(string name)
        {
            var client = new RestClient("http://localhost:5013");
            var request = new RestRequest($"api/userProfile/{name}", Method.Get);
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                // You can parse the response content to a model if needed.
                return Ok(response.Content);
            }
            else
            {
                return StatusCode((int)response.StatusCode, response.ErrorMessage);
            }
        }


        [HttpGet("searchByAccountNo")]
        public IActionResult SearchByAccountNo(uint accountNo)
        {
            var client = new RestClient("http://localhost:5013");
            var request = new RestRequest($"api/accounts/{accountNo}", Method.Get);
            var response = client.Execute<DataIntermed>(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Data);
            }
            else
            {
                return StatusCode((int)response.StatusCode, response.ErrorMessage);
            }
        }


    }
}
