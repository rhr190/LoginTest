using LoginTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace LoginTest.Controllers
{
    public class Call : Controller
    {
        public IActionResult Index(string message)
        {
            ViewBag.Message = message;
            return View();
        }

        [HttpPost]
        public IActionResult Index(string userName, string password)
        {
            if ((userName != "user") || (password != "password"))
                return View((object)"Login Failed");

            var accessToken = GenerateJSONWebToken();
            setJWTCookie(accessToken);
            return RedirectToAction("FlightReservation");
        }

        private string GenerateJSONWebToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("781fasfdejjjgoioiw323assjdhgiosuahdgisayhdgiashdgihguusfhu2m"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7117",
                audience: "https://localhost:7117",
                expires: DateTime.Now.AddMinutes(2),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void setJWTCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(2),
            };
            Response.Cookies.Append("jwtCookie", token, cookieOptions);
        }

        
        public async Task<IActionResult> FlightReservation()
        {
            var jwt = Request.Cookies["jwtCookie"];
            List<Reservation> reservationList = new List<Reservation>();
            
            using (var httpClient = new HttpClient()) 
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", jwt);
                using (var response = await httpClient.GetAsync("https://localhost:7117/Reservation"))
                {
                    if(response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        reservationList = JsonConvert.DeserializeObject<List<Reservation>>(apiResponse);
                    }

                    if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        return RedirectToAction("Index", new { message = "Please login again" });
                    }
                }
                return View(reservationList);
            }
        }
    }


}
