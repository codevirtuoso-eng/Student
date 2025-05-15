using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharedLibrary.DTO;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebAppAPI.ApiFunctions;

namespace WebAppAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private IUsersFunctions _usersFunction;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ILogger<UsersController> _logger;

		public UsersController(IUsersFunctions usersFunction, ILogger<UsersController> logger, IHttpContextAccessor httpContextAccessor)
		{
			_usersFunction = usersFunction;
			_logger = logger;
			_httpContextAccessor = httpContextAccessor;
			_logger.LogDebug("NLog injected into UsersController");
		}

		[AllowAnonymous]
		[HttpPost]
		[Route("Login")]
		public async Task<ActionResult> Login(LoginRequestDTO loginRequestDTO)
		{
			_logger.LogInformation($"Login was called with userName: {loginRequestDTO.UserName}");

			try
			{
				var result = await _usersFunction.Login(loginRequestDTO);

				var user = _httpContextAccessor.HttpContext.User;  // no claims until the user has an authorized token
				return Ok(result);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred during login.");
				var responseObject = new { responseText = ex.Message }; // Not necessarily a friendly message
				return StatusCode(StatusCodes.Status500InternalServerError, responseObject);
			}
		}

		// --- BEGIN: TEMPORARY DEMO USER SEED ENDPOINT ---
		[AllowAnonymous]
		[HttpPost]
		[Route("SeedDemoUser")]
		public async Task<IActionResult> SeedDemoUser()
		{
			var userName = "demo";
			var userPassword = "demo";

			// Check if user already exists
			var allUsers = await _usersFunction.GetAllUsers();
			if (allUsers.Any(u => u.UserName == userName))
				return Ok("Demo user already exists!");

			// Get the DbContext
			var dbContext = (DatabaseAccess.Data.Context.MainAppDbContext)HttpContext.RequestServices.GetService(typeof(DatabaseAccess.Data.Context.MainAppDbContext));
			if (dbContext == null)
				return StatusCode(500, "DbContext not available");

			var newUser = new DatabaseAccess.Data.EntityModels.AspNetUserDAO
			{
				Id = Guid.NewGuid().ToString(),
				UserName = userName,
				UserPassword = userPassword
			};
			dbContext.AspNetUsers.Add(newUser);
			await dbContext.SaveChangesAsync();

			return Ok("Demo user created: demo/demo");
		}
		// --- END: TEMPORARY DEMO USER SEED ENDPOINT ---

		[Authorize(Roles = "User, Admin")] // prefer to use enums, but requires custom attribute
		[HttpGet]
		[Route("GetAllUsers")]
		// example of specifying data annotation for individual parameters
		public async Task<ActionResult> GetAllUsers()
		{
			_logger.LogInformation($"GetAllUsers was called");

			try
			{
				var result = await _usersFunction.GetAllUsers();
				return Ok(result);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred getting all users.");
				var responseObject = new { responseText = ex.Message }; // Not necessarily a friendly message
				return StatusCode(StatusCodes.Status500InternalServerError, responseObject);
			}
		}

		[Authorize(Roles = "Admin")] // prefer to use enums, but requires custom attribute
		[HttpGet]
		[Route("GetUserRoles")]
		// example of specifying data annotation for individual parameters
		public async Task<ActionResult> GetUserRoles([Required][MinLength(1)][MaxLength(50)] string userId)
		{
			_logger.LogInformation($"GetUserRoles was called with userId: {userId}");

			try
			{
				var result = await _usersFunction.GetUserRoles(userId);
				return Ok(result);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred getting user roles.");
				var responseObject = new { responseText = ex.Message }; // Not necessarily a friendly message
				return StatusCode(StatusCodes.Status500InternalServerError, responseObject);
			}
		}

		[Authorize(Roles = "Admin")] // prefer to use enums, but requires custom attribute
		[HttpGet]
		[Route("GetUserClaims")]
		// example of specifying data annotation for individual parameters
		public async Task<ActionResult> GetUserClaims([Required][MinLength(1)][MaxLength(50)] string userId)
		{
			_logger.LogInformation($"GetUserClaims was called with userId: {userId}");

			try
			{
				var result = await _usersFunction.GetUserClaims(userId);
				return Ok(result);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred getting user claims.");
				var responseObject = new { responseText = ex.Message }; // Not necessarily a friendly message
				return StatusCode(StatusCodes.Status500InternalServerError, responseObject);
			}
		}
	}
}
