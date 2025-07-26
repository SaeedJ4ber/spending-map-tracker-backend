using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpendingTrackerDAL;
using SpendingTrackerBLL;

namespace SpendingTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<clsUserDTO> AddNewUser([FromBody] clsUserDTO newUserDTO)
        {
            if (newUserDTO == null ||  string.IsNullOrEmpty(newUserDTO.FirstName) ||
                string.IsNullOrEmpty(newUserDTO.LastName) ||
                string.IsNullOrEmpty(newUserDTO.Email) ||
             string.IsNullOrEmpty(newUserDTO.Password))
            {
                return BadRequest("Invalid user data.");
            }

            var user = new clsUser(newUserDTO);
            if (!user.Save())
                return StatusCode(500, "Could not save user.");

            newUserDTO.UserID = user.UserID;
            return CreatedAtAction(nameof(GetUserByID), new { id = user.UserID }, newUserDTO);
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<clsUserDTO> Login([FromBody] clsLoginDTO loginDTO)
        {
            var user = clsUser.Find(loginDTO.Email, loginDTO.Password);
            if (user == null)
                return Unauthorized("Invalid email or password.");

            return Ok(user.uDTO);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<clsUserDTO> GetUserByID(int id)
        {
            var user = clsUser.Find(id);
            if (user == null)
                return NotFound($"User with ID {id} not found.");

            return Ok(user.uDTO);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<clsUserDTO> UpdateUser(int id, [FromBody] clsUserDTO updatedUser)
        {
            if (id < 1 ||
                updatedUser == null ||
                string.IsNullOrEmpty(updatedUser.FirstName) ||
                string.IsNullOrEmpty(updatedUser.LastName) ||
                string.IsNullOrEmpty(updatedUser.Email))
            {
                return BadRequest("Invalid user data.");
            }

            var user = clsUser.Find(id);
            if (user == null)
                return NotFound($"User with ID {id} not found.");

            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Email = updatedUser.Email;
            user.Password = updatedUser.Password;

            if (!user.Save())
                return StatusCode(500, "Failed to update user.");

            return Ok(user.uDTO);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteUser(int id)
        {
            if (id < 1)
                return BadRequest("Invalid user ID.");

            if (!clsUser.DeleteUser(id))
                return NotFound($"User with ID {id} not found.");

            return Ok(new { message = $"User with ID {id} deleted successfully." });
        }
    }
}