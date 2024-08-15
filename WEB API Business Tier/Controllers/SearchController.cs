using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace WEB_API_Business_Tier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly string DataApiUrl = "http://localhost:5009/api/accounts";

        [HttpPost]
        public IActionResult SearchForAccount([FromBody] API_Classes.SearchData searchData)
        {
            var client = new RestClient(DataApiUrl);
            var request = new RestRequest("search", Method.Post);
            request.AddJsonBody(searchData);
            var response = client.Execute<API_Classes.DataIntermed>(request);

            if (response.IsSuccessful)
            {
                return Ok(response.Data); // Return the account details from the Data Tier based on the search
            }
            else
            {
                return StatusCode((int)response.StatusCode, response.ErrorMessage);
            }
        }
    }
}
