using itdiv_mini_project.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Claims;

namespace itdiv_mini_project.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration)
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
        //register validation with model done.
        [HttpPost]
        public string Registration(User account)
        {
            if (ModelState.IsValid)
            {
                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
                SqlCommand cmd = new SqlCommand("INSERT INTO [User] (Name, Email, Password) VALUES('" + account.Name + "', '" + account.Email + "','" + account.Password + "')", con);
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
            else {
                return "Validation failed. Please check your input.";

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
        public async Task<IActionResult> Login(User account)
        {
            //login pake validasi lagi?
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [User] WHERE Email = '" + account.Email + "' AND Password = '" + account.Password + "'", con);
            DataTable dt = new DataTable();

            
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0]; // Assuming only one record is expected
                String UserID =row["UserID"].ToString();
                string userName = row["Name"].ToString();

                var claims = new List<Claim>
                  {
                    new Claim("UserID", UserID),
                    new Claim("Name", userName)
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
