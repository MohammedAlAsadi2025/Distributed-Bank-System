using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API_Classes;
using Newtonsoft.Json;
using WEB_API_Business_Tier.Controllers;

namespace WEB_API_Business_Tier.Controllers
{ 
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        [HttpGet("defaultview")]
        public IActionResult GetDefaultView()
        {/*
            if (Request.Cookies.ContainsKey("SessionID"))
            {
                var cookieValue = Request.Cookies["SessionID"];
                if (cookieValue == "1234567")
                {
                return PartialView("LoginViewAuthenticated");
                }

            }
            // Return the partial view as HTML*/
            return PartialView("LoginDefaultView");
        }

    [HttpGet("authview")]
    public IActionResult GetLoginAuthenticatedView()
    {
        if (Request.Cookies.ContainsKey("SessionID"))
        {
            var cookieValue = Request.Cookies["SessionID"];
            if (cookieValue == "1234567")
            {
                return PartialView("LoginViewAuthenticated");
            }

        }
        // Return the partial view as HTML
        return PartialView("LoginErrorView");
    }

    [HttpGet("error")]
    public IActionResult GetLoginErrorView()
    {
        return PartialView("LoginErrorView");
    }

        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody] UserProfileIntermed user)
        {
            var response = new { login = false };

            if (user != null && !string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.Password))
            {
                // Instantiate UserProfileController and fetch the UserProfile
                var userProfileController = new UserProfileController();
                var userProfileResponse = userProfileController.GetUserProfile(user.UserName);

                if (userProfileResponse is OkObjectResult okResult)
                {
                    UserProfileIntermed dbUser = JsonConvert.DeserializeObject<UserProfileIntermed>(okResult.Value.ToString());

                    // If a user with the provided username exists and the passwords match
                    if (dbUser != null && dbUser.Password == user.Password)
                    {
                        if (dbUser.UserName == "admin" && dbUser.Password == "admin111")
                        {
                            Response.Cookies.Append("IsAdmin", "true");
                        }
                        // Set the UserName in a cookie.
                        Response.Cookies.Append("UserName", user.UserName);

                        // Continue with your existing SessionID logic.
                        Response.Cookies.Append("SessionID", "1234567");
                        response = new { login = true };
                    }
                    else
                    {
                        ViewBag.Error = "User name and Password are not matched. Try again";
                    }
                }
                else
                {
                    ViewBag.Error = "Error fetching user data.";
                }
            }
            else
            {
                ViewBag.Error = "Please provide both username and password.";
            }

            return Json(response);
        }




    }
}

