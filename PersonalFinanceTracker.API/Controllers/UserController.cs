using System;
using System.Net;
using System.Web.Http;
using PersonalFinanceTracker.Models;
using PersonalFinanceTracker.Data.Repositories;

namespace PersonalFinanceTracker.API.Controllers
{
    /// <summary>
    /// Controller for managing user-related operations
    /// </summary>
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User details</returns>
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var user = _userRepository.GetById(id);
                if (user == null)
                    return Content(HttpStatusCode.NotFound, ApiResponse<User>.CreateError("User not found"));

                return Ok(ApiResponse<User>.CreateSuccess(user));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>User details</returns>
        [HttpGet]
        [Route("by-username/{username}")]
        public IHttpActionResult GetByUsername(string username)
        {
            try
            {
                var user = _userRepository.GetByUsername(username);
                if (user == null)
                    return Content(HttpStatusCode.NotFound, ApiResponse<User>.CreateError("User not found"));

                return Ok(ApiResponse<User>.CreateSuccess(user));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email">Email address</param>
        /// <returns>User details</returns>
        [HttpGet]
        [Route("by-email/{email}")]
        public IHttpActionResult GetByEmail(string email)
        {
            try
            {
                var user = _userRepository.GetByEmail(email);
                if (user == null)
                    return Content(HttpStatusCode.NotFound, ApiResponse<User>.CreateError("User not found"));

                return Ok(ApiResponse<User>.CreateSuccess(user));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="user">User details</param>
        /// <returns>Created user details</returns>
        [HttpPost]
        [Route("register")]
        public IHttpActionResult Create([FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Check if username or email already exists
                var existingUser = _userRepository.GetByUsername(user.Username);
                if (existingUser != null)
                    return Content(HttpStatusCode.Conflict, ApiResponse<User>.CreateError("Username already exists"));

                existingUser = _userRepository.GetByEmail(user.Email);
                if (existingUser != null)
                    return Content(HttpStatusCode.Conflict, ApiResponse<User>.CreateError("Email already exists"));

                user.CreatedDate = DateTime.UtcNow;
                // Note: In a real application, you would hash the password before sending it to the repository
                var userId = _userRepository.Create(user, user.PasswordHash);
                if (userId <= 0)
                    return Content(HttpStatusCode.InternalServerError, ApiResponse<User>.CreateError("Failed to create user"));

                var createdUser = _userRepository.GetById(userId);
                return Created($"api/users/{createdUser.UserID}", ApiResponse<User>.CreateSuccess(createdUser, "User created successfully"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Update user profile
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="user">Updated user details</param>
        /// <returns>Updated user details</returns>
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Update(int id, [FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != user.UserID)
                    return BadRequest("User ID mismatch");

                var existingUser = _userRepository.GetById(id);
                if (existingUser == null)
                    return Content(HttpStatusCode.NotFound, ApiResponse<User>.CreateError("User not found"));

                // Check if new email is already taken by another user
                if (existingUser.Email != user.Email)
                {
                    var emailUser = _userRepository.GetByEmail(user.Email);
                    if (emailUser != null)
                        return Content(HttpStatusCode.Conflict, ApiResponse<User>.CreateError("Email already exists"));
                }

                var success = _userRepository.Update(user);
                if (!success)
                    return Content(HttpStatusCode.InternalServerError, ApiResponse<User>.CreateError("Failed to update user"));

                var updatedUser = _userRepository.GetById(id);
                return Ok(ApiResponse<User>.CreateSuccess(updatedUser, "User updated successfully"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Authenticate user
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>Authenticated user details</returns>
        [HttpPost]
        [Route("authenticate")]
        public IHttpActionResult Authenticate([FromBody] LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Note: In a real application, you would hash the password before sending it to the repository
                var isAuthenticated = _userRepository.Authenticate(request.Username, request.Password);
                if (!isAuthenticated)
                    return Content(HttpStatusCode.Unauthorized, ApiResponse<User>.CreateError("Invalid username or password"));

                var user = _userRepository.GetByUsername(request.Username);
                user.LastLoginDate = DateTime.UtcNow;
                _userRepository.Update(user);

                return Ok(ApiResponse<User>.CreateSuccess(user, "Authentication successful"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }

    /// <summary>
    /// Request model for user authentication
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
    }
} 