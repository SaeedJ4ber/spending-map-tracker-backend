//using Microsoft.AspNetCore.Mvc;
//using SpendingTrackerBLL;
//using SpendingTrackerDAL;

//namespace SpendingTrackerAPI.Controllers
//{
//    [Route("api/Auth")]
//    [ApiController]
//    public class AuthController : ControllerBase
//    {
//        [HttpPost("Login")]
//        public ActionResult<clsUserDTO> Login(clsUserDTO uDTO)
//        {
//            if (string.IsNullOrWhiteSpace(uDTO.Email) || string.IsNullOrWhiteSpace(uDTO.Password))
//            {
//                return BadRequest("Email and Password are required.");
//            }

//            clsUser user = clsUser.Login(uDTO.Email, uDTO.Password);

//            if (user == null)
//            {
//                return Unauthorized("Invalid email or password.");
//            }

//            return Ok(user.uDTO);
//        }
//    }
//}
