using System;
using System.Net;
using System.Web.Http;
using PersonalFinanceTracker.Models;
using PersonalFinanceTracker.Data.Repositories;

namespace PersonalFinanceTracker.API.Controllers
{
    /// <summary>
    /// Controller for managing financial transactions
    /// </summary>
    [RoutePrefix("api/transactions")]
    public class TransactionsController : ApiController
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionsController(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
        }

        /// <summary>
        /// Get transaction by ID
        /// </summary>
        /// <param name="id">Transaction ID</param>
        /// <returns>Transaction details</returns>
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var transaction = _transactionRepository.GetById(id);
                if (transaction == null)
                    return Content(HttpStatusCode.NotFound, ApiResponse<Transaction>.CreateError("Transaction not found"));

                return Ok(ApiResponse<Transaction>.CreateSuccess(transaction));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Get transactions for a user with optional filters
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="startDate">Optional start date filter</param>
        /// <param name="endDate">Optional end date filter</param>
        /// <param name="categoryId">Optional category filter</param>
        /// <param name="accountId">Optional account filter</param>
        /// <param name="isIncome">Optional income/expense filter</param>
        /// <returns>List of transactions</returns>
        [HttpGet]
        [Route("user/{userId}")]
        public IHttpActionResult GetByUserId(int userId, [FromUri] DateTime? startDate = null, [FromUri] DateTime? endDate = null,
            [FromUri] int? categoryId = null, [FromUri] int? accountId = null, [FromUri] bool? isIncome = null)
        {
            try
            {
                var transactions = _transactionRepository.GetByUserId(userId, startDate, endDate, categoryId, accountId, isIncome);
                return Ok(ApiResponse<Transaction[]>.CreateSuccess(transactions.ToArray()));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Create a new transaction
        /// </summary>
        /// <param name="transaction">Transaction details</param>
        /// <returns>Created transaction details</returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Create([FromBody] Transaction transaction)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                transaction.CreatedDate = DateTime.UtcNow;
                transaction.ModifiedDate = DateTime.UtcNow;
                var transactionId = _transactionRepository.Create(transaction);
                if (transactionId <= 0)
                    return Content(HttpStatusCode.InternalServerError, ApiResponse<Transaction>.CreateError("Failed to create transaction"));

                var createdTransaction = _transactionRepository.GetById(transactionId);
                return Created($"api/transactions/{createdTransaction.TransactionID}", ApiResponse<Transaction>.CreateSuccess(createdTransaction, "Transaction created successfully"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Update transaction details
        /// </summary>
        /// <param name="id">Transaction ID</param>
        /// <param name="transaction">Updated transaction details</param>
        /// <returns>Updated transaction details</returns>
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Update(int id, [FromBody] Transaction transaction)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != transaction.TransactionID)
                    return BadRequest("Transaction ID mismatch");

                var existingTransaction = _transactionRepository.GetById(id);
                if (existingTransaction == null)
                    return Content(HttpStatusCode.NotFound, ApiResponse<Transaction>.CreateError("Transaction not found"));

                transaction.ModifiedDate = DateTime.UtcNow;
                var success = _transactionRepository.Update(transaction);
                if (!success)
                    return Content(HttpStatusCode.InternalServerError, ApiResponse<Transaction>.CreateError("Failed to update transaction"));

                var updatedTransaction = _transactionRepository.GetById(id);
                return Ok(ApiResponse<Transaction>.CreateSuccess(updatedTransaction, "Transaction updated successfully"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Delete a transaction
        /// </summary>
        /// <param name="id">Transaction ID</param>
        /// <returns>Success status</returns>
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var existingTransaction = _transactionRepository.GetById(id);
                if (existingTransaction == null)
                    return Content(HttpStatusCode.NotFound, ApiResponse<bool>.CreateError("Transaction not found"));

                var success = _transactionRepository.Delete(id);
                if (!success)
                    return Content(HttpStatusCode.InternalServerError, ApiResponse<bool>.CreateError("Failed to delete transaction"));

                return Ok(ApiResponse<bool>.CreateSuccess(true, "Transaction deleted successfully"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
} 