using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.Password;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private IUserService UserService { get; set; }
        private IUserResolverService UserResolverService { get; set; }

        public UsersController(IUserService userService, IUserResolverService userResolverService)
        {
            this.UserService = userService;
            this.UserResolverService = userResolverService;
        }

        /// <summary>
        /// Get list of all users (only for admin)
        /// </summary>
        /// <returns></returns>
        // GET: api/<controller>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var users = await UserService.GetAllUsers();
            if (users == null) return NoContent();
            return Ok(users);
        }

        /// <summary>
        /// Get user by id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<controller>/5
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDto>> Get([FromRoute] int userId)
        {
            var user = await UserService.GetById(x=>x.Id == userId);
            if (user == null)
                return NotFound();
            return user;
        }

        // GET api/<controller>/5
        [HttpGet("id")]
        [Authorize]
        public async Task<ActionResult<int>> GetUserId()
        {
            var userId = UserResolverService.GetUserId();
            return Ok(userId);
        }

        // PUT api/<controller>/5
        /// <summary>
        /// Function for updating info about user
        /// </summary>
        /// <param name="user"></param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody]UserUpdateDto user)
        {
            if (id == UserResolverService.GetUserId() || UserResolverService.IsUserAdmin())
            {
                user.Id = id;
                await UserService.UpdateUser(user);
                return NoContent();
            }

            return Forbid();
        }

        /// <summary>
        /// Function for deleting user (only for admin)
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await UserService.RemoveUser(id);
            return Ok();
        }

        [HttpPost("password")]
        public async Task<IActionResult> ForgotPassword([FromBody]ResetPasswordDto email)
        {
            await UserService.SendPasswordResetConfirmation(email.Email);
            return Ok();
        }

        [HttpPut("password")]
        public async Task<IActionResult> CreateNewPassword([FromBody]ResetPasswordDto newPassword)
        {
            await UserService.ResetPassword(newPassword);
            return Ok();
        }

        [HttpPut("email")]
        public async Task<IActionResult> ForbidEmailNotification([FromBody]ForbidEmailDto email)
        {
            if (await UserService.ForbidEmailNotification(email))
                return Ok();
            return BadRequest();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<RegisterDto>> Register([FromBody] RegisterDto user)
        {
            var createdUser = await UserService.AddUser(user);
            if (createdUser != null)
            {
                return CreatedAtAction(nameof(GetUserId), new { id = createdUser.Id }, createdUser);
            }

            return BadRequest();
        }
    }
}
