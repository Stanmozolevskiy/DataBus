using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Xml.Linq;
using static DataAccess.DataAccess;

namespace DataBus.Controllers
{
    [Route("[controller]"), ApiController, Produces("application/xml")]
    public class UserController: ControllerBase
    {
        public UserController(IConfiguration configuration)
        {
            this.configuration = configuration;
            queryContext = new QueryContext(configuration.GetVariableByEnvironment("Database:QueryContext"),
                        300, TimeSpan.FromSeconds(10), 5);
        }

        [HttpPost("GetUserById"), Produces("application/xml")]
        public async Task<IActionResult> GetUser(XElement user)
        {
            int id = int.Parse(user.Attribute("id").Value);
            XElement response = await DataAccess.DataAccess.CRUDAsync<XElement>(queryContext, "GetUser",
                                                                                        CommandTypeEx.StoredProcedure,
                                                                                        new SqlParameter("@id", id));
            return Ok(response);
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] XElement user)
        {
            XElement response = await DataAccess.DataAccess.CRUDAsync<XElement>(queryContext, "CreateNewUser",
                                                                        CommandTypeEx.StoredProcedure,
                                                                        new SqlParameter("@user", user.ToString()));
            return Ok(response);
        }

        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] XElement user)
        {
            int id = int.Parse(user.Attribute("id").Value);

            XElement response = await DataAccess.DataAccess.CRUDAsync<XElement>(queryContext, "DeleteUser",
                                                                        CommandTypeEx.StoredProcedure,
                                                                        new SqlParameter("@user", user.ToString()));

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] XElement user)
        {
            XElement response = await DataAccess.DataAccess.CRUDAsync<XElement>(queryContext, "UpdateUser",
                                                                       CommandTypeEx.StoredProcedure,
                                                                       new SqlParameter("@user", user.ToString()));
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserList()
        {
            await Task.Yield();
            //XElement response = await DataAccess.DataAccess.CRUDAsync<XElement>(queryContext, "GetUserList",
            //                                                           CommandTypeEx.StoredProcedure);
            return Ok(configuration.GetVariableByEnvironment("Database:QueryContext") + " _______" + "_____" + Environment.GetEnvironmentVariable("MSSQL_URL"));
        }


        private IConfiguration configuration;
        private QueryContext queryContext;
    }

}

