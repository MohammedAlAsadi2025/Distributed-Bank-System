using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using API_Classes;

namespace WEB_API_Business_Tier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly string DataApiUrl = "http://localhost:5009/api/UserProfile";

        [HttpGet]
        public IActionResult GetAllUserProfiles()
        {
            var client = new RestClient(DataApiUrl);
            var request = new RestRequest("", Method.Get);
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

        [HttpGet("{userName}")]
        public IActionResult GetUserProfile(string userName)
        {
            var client = new RestClient(DataApiUrl);
            var request = new RestRequest($"{userName}", Method.Get);
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
        public IActionResult CreateUserProfile([FromBody] UserProfileIntermed newUserProfile)
        {
            var client = new RestClient(DataApiUrl);
            var request = new RestRequest("", Method.Post);

            request.AddJsonBody(newUserProfile);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return Ok("Successfully inserted");
            }
            else
            {
                return BadRequest(response.Content);
            }
        }

        [HttpPut("{userName}")]
        public IActionResult UpdateUserProfile(string userName, [FromBody] UserProfileIntermed userProfileData)
        {
            var client = new RestClient(DataApiUrl);
            var request = new RestRequest($"{userName}", Method.Put);
            request.AddJsonBody(userProfileData);

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

        [HttpDelete("{userName}")]
        public IActionResult DeleteUserProfile(string userName)
        {
            var client = new RestClient(DataApiUrl);
            var request = new RestRequest($"{userName}", Method.Delete);

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