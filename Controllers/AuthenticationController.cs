using JWTTokenAPI.Models;
using JWTTokenAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace JWTTokenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthenticationController> _logger;
     

        public AuthenticationController(IAuthService authService, ILogger<AuthenticationController> logger)
        {
            _authService = authService;
          
            _logger = logger;
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid payload");
                var (status,user, message) = await _authService.Login(model);
                if (status == 0)
                    return BadRequest(message);
                return Ok(new LoggedUser(user, message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

       

        [HttpPost]
        [Route("register")]       
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid payload");
                var (status, message) = 
                    await _authService.Register(model);
                if (status == 0)
                {
                    return BadRequest(message);
                }
                return CreatedAtAction(nameof(Register), model);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        } 
        
        //[HttpPost]
        //[Route("update")]
        //[Authorize]
        //public async Task<IActionResult> Update(UpdateModel model)
        //{
        //    try
        //    {
        //        var currentUserName = HttpContext.User.Identity.Name;

        //        var user = await _userManager.FindByIdAsync(model.Id);
        //        var currentUser = await _userManager.FindByNameAsync(currentUserName);
        //        var roles = await _userManager.GetRolesAsync(currentUser);
        //        if (user.UserName.Equals(currentUserName) || roles.IndexOf("SAdmin") != -1)
        //        {
        //            if (!ModelState.IsValid)
        //                return BadRequest("Invalid payload");
        //            var (status, message) =
        //                await _authService.Update(model);
        //            if (status == 0)
        //            {
        //                return BadRequest(message);
        //            }
        //            return Ok(message);
        //        }
        //        return BadRequest("Unauthorized");
               
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //}  
        
       
    }
}
