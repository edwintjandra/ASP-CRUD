using itdiv_mini_project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace itdiv_mini_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("registration")]
        public string registration(Account account) 
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            SqlCommand cmd = new SqlCommand("INSERT INTO Account( Name, Email,Password) VALUES('"+account.Name+"' , '"+ account.Email +"','"+account.Password+"')", con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                return "data inserted succesfully";
            } else
            {
                return "error";
            }
        }

        [HttpPost]
        [Route("login")]
        public string login(Account account) {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Account WHERE Email = '"+account.Email+"' And Password= '"+account.Password+"'  ",con);
            DataTable dt = new DataTable();

            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return "data found";
            } else 
            {
                return "invalid user";
            }
        }
    }
}
