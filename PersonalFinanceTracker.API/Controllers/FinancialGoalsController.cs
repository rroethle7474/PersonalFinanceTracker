using System;
using System.Net;
using System.Web.Http;
using PersonalFinanceTracker.Models;
using PersonalFinanceTracker.Data.Repositories;

namespace PersonalFinanceTracker.API.Controllers
{
    /// <summary>
    /// Controller for managing financial goals
    /// </summary>
    [RoutePrefix("api/goals")]
    public class FinancialGoalsController : ApiController
    {
        private readonly IFinancialGoalRepository _goalRepository;

        public FinancialGoalsController(IFinancialGoalRepository goalRepository)
        {
            _goalRepository = goalRepository ?? throw new ArgumentNullException(nameof(goalRepository));
        }

        /// <summary>
        /// Get goal by ID
        /// </summary>
        /// <param name="id">Goal ID</param>
        /// <returns>Goal details</returns>
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var goal = _goalRepository.GetById(id);
                if (goal == null)
                    return Content(HttpStatusCode.NotFound, ApiResponse<FinancialGoal>.CreateError("Goal not found"));

                return Ok(ApiResponse<FinancialGoal>.CreateSuccess(goal));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Get goals for a user with optional filters
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="category">Optional category filter</param>
        /// <param name="status">Optional status filter</param>
        /// <returns>List of goals</returns>
        [HttpGet]
        [Route("user/{userId}")]
        public IHttpActionResult GetByUserId(int userId, [FromUri] string category = null, [FromUri] string status = null)
        {
            try
            {
                var goals = _goalRepository.GetByUserId(userId, category, status);
                return Ok(ApiResponse<FinancialGoal[]>.CreateSuccess(goals.ToArray()));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Create a new goal
        /// </summary>
        /// <param name="goal">Goal details</param>
        /// <returns>Created goal details</returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Create([FromBody] FinancialGoal goal)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var goalId = _goalRepository.Create(goal);
                if (goalId <= 0)
                    return Content(HttpStatusCode.InternalServerError, ApiResponse<FinancialGoal>.CreateError("Failed to create goal"));

                var createdGoal = _goalRepository.GetById(goalId);
                return Created($"api/goals/{createdGoal.GoalID}", ApiResponse<FinancialGoal>.CreateSuccess(createdGoal, "Goal created successfully"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Update goal details
        /// </summary>
        /// <param name="id">Goal ID</param>
        /// <param name="goal">Updated goal details</param>
        /// <returns>Updated goal details</returns>
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Update(int id, [FromBody] FinancialGoal goal)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != goal.GoalID)
                    return BadRequest("Goal ID mismatch");

                var existingGoal = _goalRepository.GetById(id);
                if (existingGoal == null)
                    return Content(HttpStatusCode.NotFound, ApiResponse<FinancialGoal>.CreateError("Goal not found"));

                var success = _goalRepository.Update(goal);
                if (!success)
                    return Content(HttpStatusCode.InternalServerError, ApiResponse<FinancialGoal>.CreateError("Failed to update goal"));

                var updatedGoal = _goalRepository.GetById(id);
                return Ok(ApiResponse<FinancialGoal>.CreateSuccess(updatedGoal, "Goal updated successfully"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Update goal progress
        /// </summary>
        /// <param name="id">Goal ID</param>
        /// <param name="newAmount">New current amount</param>
        /// <param name="newStatus">Optional new status</param>
        /// <returns>Updated goal details</returns>
        [HttpPut]
        [Route("{id}/progress")]
        public IHttpActionResult UpdateProgress(int id, [FromBody] decimal newAmount, [FromUri] string newStatus = null)
        {
            try
            {
                var existingGoal = _goalRepository.GetById(id);
                if (existingGoal == null)
                    return Content(HttpStatusCode.NotFound, ApiResponse<FinancialGoal>.CreateError("Goal not found"));

                var success = _goalRepository.UpdateProgress(id, newAmount, newStatus);
                if (!success)
                    return Content(HttpStatusCode.InternalServerError, ApiResponse<FinancialGoal>.CreateError("Failed to update goal progress"));

                var updatedGoal = _goalRepository.GetById(id);
                return Ok(ApiResponse<FinancialGoal>.CreateSuccess(updatedGoal, "Goal progress updated successfully"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Delete a goal
        /// </summary>
        /// <param name="id">Goal ID</param>
        /// <returns>Success status</returns>
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var existingGoal = _goalRepository.GetById(id);
                if (existingGoal == null)
                    return Content(HttpStatusCode.NotFound, ApiResponse<bool>.CreateError("Goal not found"));

                var success = _goalRepository.Delete(id);
                if (!success)
                    return Content(HttpStatusCode.InternalServerError, ApiResponse<bool>.CreateError("Failed to delete goal"));

                return Ok(ApiResponse<bool>.CreateSuccess(true, "Goal deleted successfully"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Sync goal from Salesforce
        /// </summary>
        /// <param name="goal">Goal details from Salesforce</param>
        /// <returns>Success status</returns>
        [HttpPost]
        [Route("sync")]
        public IHttpActionResult SyncFromSalesforce([FromBody] FinancialGoal goal)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (string.IsNullOrEmpty(goal.SalesforceID))
                    return BadRequest("SalesforceID is required for sync");

                var success = _goalRepository.SyncFromSalesforce(goal);
                if (!success)
                    return Content(HttpStatusCode.InternalServerError, ApiResponse<bool>.CreateError("Failed to sync goal from Salesforce"));

                return Ok(ApiResponse<bool>.CreateSuccess(true, "Goal synced successfully"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
} 