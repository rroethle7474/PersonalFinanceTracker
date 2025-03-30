using System;
using System.Net;
using System.Web.Http;
using PersonalFinanceTracker.Models;
using PersonalFinanceTracker.Data.Repositories;

namespace PersonalFinanceTracker.API.Controllers
{
    /// <summary>
    /// Controller for managing budget categories
    /// </summary>
    [RoutePrefix("api/categories")]
    public class BudgetCategoriesController : ApiController
    {
        private readonly IBudgetCategoryRepository _categoryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BudgetCategoriesController"/> class.
        /// </summary>
        /// <param name="categoryRepository">The budget categories repository.</param>
        /// <exception cref="ArgumentNullException">Thrown when Budget Category Repository is null.</exception>
        public BudgetCategoriesController(IBudgetCategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        /// <summary>
        /// Get category by ID
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>Category details</returns>
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var category = _categoryRepository.GetById(id);
                if (category == null)
                    return Content(HttpStatusCode.NotFound, ApiResponse<BudgetCategory>.CreateError("Category not found"));

                return Ok(ApiResponse<BudgetCategory>.CreateSuccess(category));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Get all categories for a user with optional type filter
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="categoryType">Optional category type filter (Income, Expense)</param>
        /// <returns>List of categories</returns>
        [HttpGet]
        [Route("user/{userId}")]
        public IHttpActionResult GetByUserId(int userId, [FromUri] string categoryType = null)
        {
            try
            {
                var categories = _categoryRepository.GetByUserId(userId, categoryType);
                return Ok(ApiResponse<BudgetCategory[]>.CreateSuccess(categories.ToArray()));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Create a new category
        /// </summary>
        /// <param name="category">Category details</param>
        /// <returns>Created category details</returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Create([FromBody] BudgetCategory category)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var categoryId = _categoryRepository.Create(category);
                if (categoryId <= 0)
                    return Content(HttpStatusCode.InternalServerError, ApiResponse<BudgetCategory>.CreateError("Failed to create category"));

                var createdCategory = _categoryRepository.GetById(categoryId);
                return Created($"api/categories/{createdCategory.CategoryID}", ApiResponse<BudgetCategory>.CreateSuccess(createdCategory, "Category created successfully"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Update category details
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <param name="category">Updated category details</param>
        /// <returns>Updated category details</returns>
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Update(int id, [FromBody] BudgetCategory category)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != category.CategoryID)
                    return BadRequest("Category ID mismatch");

                var existingCategory = _categoryRepository.GetById(id);
                if (existingCategory == null)
                    return Content(HttpStatusCode.NotFound, ApiResponse<BudgetCategory>.CreateError("Category not found"));

                var success = _categoryRepository.Update(category);
                if (!success)
                    return Content(HttpStatusCode.InternalServerError, ApiResponse<BudgetCategory>.CreateError("Failed to update category"));

                var updatedCategory = _categoryRepository.GetById(id);
                return Ok(ApiResponse<BudgetCategory>.CreateSuccess(updatedCategory, "Category updated successfully"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Delete a category
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>Success status</returns>
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var existingCategory = _categoryRepository.GetById(id);
                if (existingCategory == null)
                    return Content(HttpStatusCode.NotFound, ApiResponse<bool>.CreateError("Category not found"));

                var success = _categoryRepository.Delete(id);
                if (!success)
                    return Content(HttpStatusCode.InternalServerError, ApiResponse<bool>.CreateError("Failed to delete category"));

                return Ok(ApiResponse<bool>.CreateSuccess(true, "Category deleted successfully"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Sync category from Salesforce
        /// </summary>
        /// <param name="category">Category details from Salesforce</param>
        /// <returns>Success status</returns>
        [HttpPost]
        [Route("sync")]
        public IHttpActionResult SyncFromSalesforce([FromBody] BudgetCategory category)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (string.IsNullOrEmpty(category.SalesforceID))
                    return BadRequest("SalesforceID is required for sync");

                var success = _categoryRepository.SyncFromSalesforce(category);
                if (!success)
                    return Content(HttpStatusCode.InternalServerError, ApiResponse<bool>.CreateError("Failed to sync category from Salesforce"));

                return Ok(ApiResponse<bool>.CreateSuccess(true, "Category synced successfully"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
} 