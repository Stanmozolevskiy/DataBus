using Microsoft.AspNetCore.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace DataBus.Controllers
{
    [Route("[controller]"), ApiController]
    public class UserController: ControllerBase
    {
        [HttpPost("GetUserById"), Produces("application/xml")]
        public IActionResult GetUser(XElement user)
        {
            string userId = user.Element("Id").Value;

            return Ok(getUserById(int.Parse(userId)));
        }

        [HttpPost("CreateUser"), Produces("application/xml")]
        public IActionResult CreateUser([FromBody] XElement user)
        {

            string id = user.Element("Id").Value;
            var isExist = doesUserexist(int.Parse(id));

            // check if user exists 
            if (isExist)
                return Ok("The user already in the Database");

            XElement users = XDocument.Load(@"UserLIst.xml").Root;

            // if not create user 
            users.Add(user);



            return Ok("User successfuly added");
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

        private bool doesUserexist(int userId)
        {
            List<XElement> users = XDocument.Load(@"UserLIst.xml").Root.Elements("User").ToList();
            XElement user = users.FirstOrDefault(x => int.Parse(x.Element("Id").Value) == userId);

            if (user == null)
                return false;

            return true;
        }
    }
}
