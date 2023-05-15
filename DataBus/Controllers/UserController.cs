using Microsoft.AspNetCore.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace DataBus.Controllers
{
    [Route("[controller]"), ApiController]
    public class UserController: ControllerBase
    {
        [HttpPost("GetUserById"), Produces("application/xml")]
        public IActionResult GetUser([FromBody] XElement user)
        {
            string userId = user.Element("Id").Value;
            return Ok(getUserById(int.Parse(userId)));
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] XElement user)
        {
            var users = XDocument.Load(@"UserLIst.xml").Root.Elements("Users");
            // check if user exists 
            // if not create user 
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteUser([FromBody] XElement user)
        {
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateUser([FromBody] XElement user)
        {
            return Ok();
        }

        private XElement getUserById(int userId)
        {
            List<XElement> users = XDocument.Load(@"UserLIst.xml").Root.Elements("User").ToList();

            return users.FirstOrDefault(x => int.Parse(x.Element("Id").Value) == userId);
        }
    }
}
