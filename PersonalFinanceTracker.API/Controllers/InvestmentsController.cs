using System;
using System.Net;
using System.Web.Http;
using PersonalFinanceTracker.Models;
using PersonalFinanceTracker.Data.Repositories;

namespace PersonalFinanceTracker.API.Controllers
{
    /// <summary>
    /// Controller for managing investment holdings
    /// </summary>
    [RoutePrefix("api/investments")]
    public class InvestmentsController : ApiController
    {
        private readonly IInvestmentRepository _investmentRepository;

        public InvestmentsController(IInvestmentRepository investmentRepository)
        {
            _investmentRepository = investmentRepository ?? throw new ArgumentNullException(nameof(investmentRepository));
        }

        /// <summary>
        /// Get investment by ID
        /// </summary>
        /// <param name="id">Investment ID</param>
        /// <returns>Investment details</returns>
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var investment = _investmentRepository.GetById(id);
                if (investment == null)
                    return Content(HttpStatusCode.NotFound, ApiResponse<Investment>.CreateError("Investment not found"));

                return Ok(ApiResponse<Investment>.CreateSuccess(investment));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Get investments for a user with optional asset class filter
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="assetClass">Optional asset class filter</param>
        /// <returns>List of investments</returns>
        [HttpGet]
        [Route("user/{userId}")]
        public IHttpActionResult GetByUserId(int userId, [FromUri] string assetClass = null)
        {
            try
            {
                var investments = _investmentRepository.GetByUserId(userId, assetClass);
                return Ok(ApiResponse<Investment[]>.CreateSuccess(investments.ToArray()));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Create a new investment
        /// </summary>
        /// <param name="investment">Investment details</param>
        /// <returns>Created investment details</returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Create([FromBody] Investment investment)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                investment.LastUpdated = DateTime.UtcNow;
                var investmentId = _investmentRepository.Create(investment);
                if (investmentId <= 0)
                    return Content(HttpStatusCode.InternalServerError, ApiResponse<Investment>.CreateError("Failed to create investment"));

                var createdInvestment = _investmentRepository.GetById(investmentId);
                return Created($"api/investments/{createdInvestment.InvestmentID}", ApiResponse<Investment>.CreateSuccess(createdInvestment, "Investment created successfully"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Update investment details
        /// </summary>
        /// <param name="id">Investment ID</param>
        /// <param name="investment">Updated investment details</param>
        /// <returns>Updated investment details</returns>
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Update(int id, [FromBody] Investment investment)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != investment.InvestmentID)
                    return BadRequest("Investment ID mismatch");

                var existingInvestment = _investmentRepository.GetById(id);
                if (existingInvestment == null)
                    return Content(HttpStatusCode.NotFound, ApiResponse<Investment>.CreateError("Investment not found"));

                investment.LastUpdated = DateTime.UtcNow;
                var success = _investmentRepository.Update(investment);
                if (!success)
                    return Content(HttpStatusCode.InternalServerError, ApiResponse<Investment>.CreateError("Failed to update investment"));

                var updatedInvestment = _investmentRepository.GetById(id);
                return Ok(ApiResponse<Investment>.CreateSuccess(updatedInvestment, "Investment updated successfully"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Update investment price
        /// </summary>
        /// <param name="id">Investment ID</param>
        /// <param name="newPrice">New current price</param>
        /// <returns>Updated investment details</returns>
        [HttpPut]
        [Route("{id}/price")]
        public IHttpActionResult UpdatePrice(int id, [FromBody] decimal newPrice)
        {
            try
            {
                var existingInvestment = _investmentRepository.GetById(id);
                if (existingInvestment == null)
                    return Content(HttpStatusCode.NotFound, ApiResponse<Investment>.CreateError("Investment not found"));

                var success = _investmentRepository.UpdatePrice(id, newPrice);
                if (!success)
                    return Content(HttpStatusCode.InternalServerError, ApiResponse<Investment>.CreateError("Failed to update investment price"));

                var updatedInvestment = _investmentRepository.GetById(id);
                return Ok(ApiResponse<Investment>.CreateSuccess(updatedInvestment, "Investment price updated successfully"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Delete an investment
        /// </summary>
        /// <param name="id">Investment ID</param>
        /// <returns>Success status</returns>
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var existingInvestment = _investmentRepository.GetById(id);
                if (existingInvestment == null)
                    return Content(HttpStatusCode.NotFound, ApiResponse<bool>.CreateError("Investment not found"));

                var success = _investmentRepository.Delete(id);
                if (!success)
                    return Content(HttpStatusCode.InternalServerError, ApiResponse<bool>.CreateError("Failed to delete investment"));

                return Ok(ApiResponse<bool>.CreateSuccess(true, "Investment deleted successfully"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
} 