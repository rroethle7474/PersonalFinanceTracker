using System;
using System.Net;
using System.Web.Http;
using PersonalFinanceTracker.Models;
using PersonalFinanceTracker.Data.Repositories;

namespace PersonalFinanceTracker.API.Controllers
{
    /// <summary>
    /// Controller for managing payment methods
    /// </summary>
    [RoutePrefix("api/payment-methods")]
    public class PaymentMethodsController : ApiController
    {
        private readonly IPaymentMethodRepository _paymentMethodRepository;

        public PaymentMethodsController(IPaymentMethodRepository paymentMethodRepository)
        {
            _paymentMethodRepository = paymentMethodRepository ?? throw new ArgumentNullException(nameof(paymentMethodRepository));
        }

        /// <summary>
        /// Get payment method by ID
        /// </summary>
        /// <param name="id">Payment method ID</param>
        /// <returns>Payment method details</returns>
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var paymentMethod = _paymentMethodRepository.GetById(id);
                if (paymentMethod == null)
                    return Content(HttpStatusCode.NotFound, ApiResponse<PaymentMethod>.CreateError("Payment method not found"));

                return Ok(ApiResponse<PaymentMethod>.CreateSuccess(paymentMethod));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Get payment methods for a user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>List of payment methods</returns>
        [HttpGet]
        [Route("user/{userId}")]
        public IHttpActionResult GetByUserId(int userId)
        {
            try
            {
                var paymentMethods = _paymentMethodRepository.GetByUserId(userId);
                return Ok(ApiResponse<PaymentMethod[]>.CreateSuccess(paymentMethods.ToArray()));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Create a new payment method
        /// </summary>
        /// <param name="paymentMethod">Payment method details</param>
        /// <returns>Created payment method details</returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Create([FromBody] PaymentMethod paymentMethod)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var paymentMethodId = _paymentMethodRepository.Create(paymentMethod);
                if (paymentMethodId <= 0)
                    return Content(HttpStatusCode.InternalServerError, ApiResponse<PaymentMethod>.CreateError("Failed to create payment method"));

                var createdPaymentMethod = _paymentMethodRepository.GetById(paymentMethodId);
                return Created($"api/payment-methods/{createdPaymentMethod.PaymentMethodID}", ApiResponse<PaymentMethod>.CreateSuccess(createdPaymentMethod, "Payment method created successfully"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Update payment method details
        /// </summary>
        /// <param name="id">Payment method ID</param>
        /// <param name="paymentMethod">Updated payment method details</param>
        /// <returns>Updated payment method details</returns>
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Update(int id, [FromBody] PaymentMethod paymentMethod)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != paymentMethod.PaymentMethodID)
                    return BadRequest("Payment method ID mismatch");

                var existingPaymentMethod = _paymentMethodRepository.GetById(id);
                if (existingPaymentMethod == null)
                    return Content(HttpStatusCode.NotFound, ApiResponse<PaymentMethod>.CreateError("Payment method not found"));

                var success = _paymentMethodRepository.Update(paymentMethod);
                if (!success)
                    return Content(HttpStatusCode.InternalServerError, ApiResponse<PaymentMethod>.CreateError("Failed to update payment method"));

                var updatedPaymentMethod = _paymentMethodRepository.GetById(id);
                return Ok(ApiResponse<PaymentMethod>.CreateSuccess(updatedPaymentMethod, "Payment method updated successfully"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Delete a payment method
        /// </summary>
        /// <param name="id">Payment method ID</param>
        /// <returns>Success status</returns>
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var existingPaymentMethod = _paymentMethodRepository.GetById(id);
                if (existingPaymentMethod == null)
                    return Content(HttpStatusCode.NotFound, ApiResponse<bool>.CreateError("Payment method not found"));

                var success = _paymentMethodRepository.Delete(id);
                if (!success)
                    return Content(HttpStatusCode.InternalServerError, ApiResponse<bool>.CreateError("Failed to delete payment method"));

                return Ok(ApiResponse<bool>.CreateSuccess(true, "Payment method deleted successfully"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Sync payment method from Salesforce
        /// </summary>
        /// <param name="paymentMethod">Payment method details from Salesforce</param>
        /// <returns>Success status</returns>
        [HttpPost]
        [Route("sync")]
        public IHttpActionResult SyncFromSalesforce([FromBody] PaymentMethod paymentMethod)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (string.IsNullOrEmpty(paymentMethod.SalesforceID))
                    return BadRequest("SalesforceID is required for sync");

                var success = _paymentMethodRepository.SyncFromSalesforce(paymentMethod);
                if (!success)
                    return Content(HttpStatusCode.InternalServerError, ApiResponse<bool>.CreateError("Failed to sync payment method from Salesforce"));

                return Ok(ApiResponse<bool>.CreateSuccess(true, "Payment method synced successfully"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
} 