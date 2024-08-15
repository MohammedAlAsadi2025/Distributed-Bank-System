using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_API_Data_Server.Data;
using Web_API_Data_Server.Models.UserProfile;

namespace Web_API_Data_Server.Controllers
{
    
        [Route("api/[controller]")]
        [ApiController]
        public class UserProfileController : ControllerBase
        {
            // GET: api/UserProfile
            [HttpGet]
            public ActionResult<IEnumerable<UserProfile>> GetAllUserProfiles()
            {
                return Ok(DBManager.GetAllUserProfiles());
            }

            // GET: api/UserProfile/username
            [HttpGet("{userName}")]
            public ActionResult<UserProfile> GetByUserName(string userName)
            {
                var user = DBManager.GetByUserName(userName);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }

        [HttpGet("email/{email}")]
        public IActionResult GetByEmail(string email)
        {
            UserProfile userProfile = DBManager.GetByEmail(email);
            if (userProfile != null)
            {
                return Ok(userProfile);
            }
            else
            {
                return NotFound($"No user found with email: {email}");
            }
        }

        // POST: api/UserProfile
        [HttpPost]
            public ActionResult<UserProfile> InsertUserProfile(UserProfile user)
            {
                if (DBManager.InsertUserProfile(user))
                {
                    return CreatedAtAction(nameof(GetByUserName), new { userName = user.UserName }, user);
                }
                return BadRequest("Error inserting user profile.");
            }

            // PUT: api/UserProfile/username
            [HttpPut("{userName}")]
            public IActionResult UpdateUserProfile(string userName, UserProfile user)
            {
                if (userName != user.UserName)
                {
                    return BadRequest();
                }

                if (DBManager.UpdateUserProfile(user))
                {
                    return NoContent();
                }
                return NotFound();
            }

            // DELETE: api/UserProfile/username
            [HttpDelete("{userName}")]
            public IActionResult DeleteUserProfile(string userName)
            {
                if (DBManager.DeleteUserProfile(userName))
                {
                    return NoContent();
                }
                return NotFound();
            }
        }
}


