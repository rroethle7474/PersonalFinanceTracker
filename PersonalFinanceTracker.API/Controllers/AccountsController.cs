using System;
using System.Net;
using System.Web.Http;
using PersonalFinanceTracker.Models;
using PersonalFinanceTracker.Data.Repositories;

namespace PersonalFinanceTracker.API.Controllers
{
    /// <summary>
    /// Controller for managing financial accounts
    /// </summary>
    [RoutePrefix("api/accounts")]
    public class AccountsController : ApiController
    {
        private readonly IAccountRepository _accountRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountsController"/> class.
        /// </summary>
        /// <param name="accountRepository">The account repository.</param>
        /// <exception cref="ArgumentNullException">Thrown when accountRepository is null.</exception>
        public AccountsController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        /// <summary>
        /// Get account by ID
        /// </summary>
        /// <param name="id">Account ID</param>
        /// <returns>Account details</returns>
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var account = _accountRepository.GetById(id);
                if (account == null)
                    return Content(HttpStatusCode.NotFound, ApiResponse<Account>.CreateError("Account not found"));

                return Ok(ApiResponse<Account>.CreateSuccess(account));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Get all accounts for a user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>List of accounts</returns>
        [HttpGet]
        [Route("user/{userId}")]
        public IHttpActionResult GetByUserId(int userId)
        {
            try
            {
                var accounts = _accountRepository.GetByUserId(userId);
                return Ok(ApiResponse<Account[]>.CreateSuccess(accounts.ToArray()));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Create a new account
        /// </summary>
        /// <param name="account">Account details</param>
        /// <returns>Created account details</returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Create([FromBody] Account account)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                account.LastUpdated = DateTime.UtcNow;
                var accountId = _accountRepository.Create(account);
                if (accountId <= 0)
                    return Content(HttpStatusCode.InternalServerError, ApiResponse<Account>.CreateError("Failed to create account"));

                var createdAccount = _accountRepository.GetById(accountId);
                return Created($"api/accounts/{createdAccount.AccountID}", ApiResponse<Account>.CreateSuccess(createdAccount, "Account created successfully"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Update account details
        /// </summary>
        /// <param name="id">Account ID</param>
        /// <param name="account">Updated account details</param>
        /// <returns>Updated account details</returns>
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Update(int id, [FromBody] Account account)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != account.AccountID)
                    return BadRequest("Account ID mismatch");

                var existingAccount = _accountRepository.GetById(id);
                if (existingAccount == null)
                    return Content(HttpStatusCode.NotFound, ApiResponse<Account>.CreateError("Account not found"));

                account.LastUpdated = DateTime.UtcNow;
                var success = _accountRepository.Update(account);
                if (!success)
                    return Content(HttpStatusCode.InternalServerError, ApiResponse<Account>.CreateError("Failed to update account"));

                var updatedAccount = _accountRepository.GetById(id);
                return Ok(ApiResponse<Account>.CreateSuccess(updatedAccount, "Account updated successfully"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Update account balance
        /// </summary>
        /// <param name="id">Account ID</param>
        /// <param name="newBalance">New balance amount</param>
        /// <returns>Updated account details</returns>
        [HttpPut]
        [Route("{id}/balance")]
        public IHttpActionResult UpdateBalance(int id, [FromBody] decimal newBalance)
        {
            try
            {
                var existingAccount = _accountRepository.GetById(id);
                if (existingAccount == null)
                    return Content(HttpStatusCode.NotFound, ApiResponse<Account>.CreateError("Account not found"));

                var success = _accountRepository.UpdateBalance(id, newBalance);
                if (!success)
                    return Content(HttpStatusCode.InternalServerError, ApiResponse<Account>.CreateError("Failed to update account balance"));

                var updatedAccount = _accountRepository.GetById(id);
                return Ok(ApiResponse<Account>.CreateSuccess(updatedAccount, "Account balance updated successfully"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Delete an account
        /// </summary>
        /// <param name="id">Account ID</param>
        /// <returns>Success status</returns>
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var existingAccount = _accountRepository.GetById(id);
                if (existingAccount == null)
                    return Content(HttpStatusCode.NotFound, ApiResponse<bool>.CreateError("Account not found"));

                var success = _accountRepository.Delete(id);
                if (!success)
                    return Content(HttpStatusCode.InternalServerError, ApiResponse<bool>.CreateError("Failed to delete account"));

                return Ok(ApiResponse<bool>.CreateSuccess(true, "Account deleted successfully"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
} 