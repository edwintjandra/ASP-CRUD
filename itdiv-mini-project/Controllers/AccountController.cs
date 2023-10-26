using itdiv_mini_project.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Claims;

namespace itdiv_mini_project.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

         public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View(); 
        }

        //registration functionallity
        [HttpPost]
        public string Registration(Account account)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            SqlCommand cmd = new SqlCommand("INSERT INTO Account(Name, Email, Password) VALUES('" + account.Name + "', '" + account.Email + "','" + account.Password + "')", con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                return "Data inserted successfully";
            }
            else
            {
                return "Error";
            }
        }

        public string seeMyUser() {
            var userIdClaim = User.FindFirst("UserID");
            if (userIdClaim != null)
            {
                string userId = userIdClaim.Value;
                // Use the userId as needed
                return userId;
            }
            else {
                return "ora ono";
            }
        }
        //login functionality 
        [HttpPost]
        public async Task<IActionResult> Login(Account account)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Account WHERE Email = '" + account.Email + "' AND Password = '" + account.Password + "'", con);
            DataTable dt = new DataTable();

            
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0]; // Assuming only one record is expected
                String UserID =row["UserID"].ToString();
                
                var claims = new List<Claim>
                  {
                    new Claim("UserID", UserID)
                  };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));

                return Redirect("/"); // Redirect to the desired page
            }
            else
            {
                return View("LoginError"); // Display a login error view
            }
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }


    }
}
