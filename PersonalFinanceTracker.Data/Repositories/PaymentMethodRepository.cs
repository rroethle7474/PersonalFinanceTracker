using PersonalFinanceTracker.Models;
using System.Collections.Generic;
using System.Data;

namespace PersonalFinanceTracker.Data.Repositories
{
    /// <summary>
    /// Repository for PaymentMethod data operations
    /// </summary>
    public class PaymentMethodRepository : BaseRepository, IPaymentMethodRepository
    {
        /// <summary>
        /// Creates a new instance of PaymentMethodRepository
        /// </summary>
        public PaymentMethodRepository(IDatabaseFactory dbFactory) : base(dbFactory)
        {
        }

        /// <summary>
        /// Gets a payment method by ID
        /// </summary>
        public PaymentMethod GetById(int paymentMethodId)
        {
            using (var db = CreateContext())
            {
                var parameter = CreateParameter("@PaymentMethodID", paymentMethodId);
                var dt = db.ExecuteStoredProcedure("usp_GetPaymentMethodById", parameter);

                if (dt.Rows.Count == 0)
                    return null;

                return MapDataRowToPaymentMethod(dt.Rows[0]);
            }
        }

        /// <summary>
        /// Gets payment methods by user ID
        /// </summary>
        public List<PaymentMethod> GetByUserId(int userId)
        {
            using (var db = CreateContext())
            {
                var parameter = CreateParameter("@UserID", userId);
                var dt = db.ExecuteStoredProcedure("usp_GetPaymentMethods", parameter);

                var paymentMethods = new List<PaymentMethod>();
                foreach (DataRow row in dt.Rows)
                {
                    paymentMethods.Add(MapDataRowToPaymentMethod(row));
                }

                return paymentMethods;
            }
        }

        /// <summary>
        /// Creates a new payment method
        /// </summary>
        public int Create(PaymentMethod paymentMethod)
        {
            using (var db = CreateContext())
            {
                var parameters = new[]
                {
                    CreateParameter("@UserID", paymentMethod.UserID),
                    CreateParameter("@MethodName", paymentMethod.MethodName),
                    CreateParameter("@MethodType", paymentMethod.MethodType),
                    CreateParameter("@CurrentBalance", paymentMethod.CurrentBalance),
                    CreateParameter("@IsActive", paymentMethod.IsActive),
                    CreateParameter("@SalesforceID", paymentMethod.SalesforceID)
                };

                var result = db.ExecuteScalar("usp_CreatePaymentMethod", parameters);
                return ToInt(result);
            }
        }

        /// <summary>
        /// Updates an existing payment method
        /// </summary>
        public bool Update(PaymentMethod paymentMethod)
        {
            using (var db = CreateContext())
            {
                var parameters = new[]
                {
                    CreateParameter("@PaymentMethodID", paymentMethod.PaymentMethodID),
                    CreateParameter("@MethodName", paymentMethod.MethodName),
                    CreateParameter("@MethodType", paymentMethod.MethodType),
                    CreateParameter("@CurrentBalance", paymentMethod.CurrentBalance),
                    CreateParameter("@IsActive", paymentMethod.IsActive)
                };

                return db.ExecuteNonQuery("usp_UpdatePaymentMethod", parameters) > 0;
            }
        }

        /// <summary>
        /// Deletes a payment method
        /// </summary>
        public bool Delete(int paymentMethodId)
        {
            using (var db = CreateContext())
            {
                var parameter = CreateParameter("@PaymentMethodID", paymentMethodId);
                return db.ExecuteNonQuery("usp_DeletePaymentMethod", parameter) > 0;
            }
        }

        /// <summary>
        /// Syncs a payment method from Salesforce
        /// </summary>
        public bool SyncFromSalesforce(PaymentMethod paymentMethod)
        {
            using (var db = CreateContext())
            {
                var parameters = new[]
                {
                    CreateParameter("@UserID", paymentMethod.UserID),
                    CreateParameter("@SalesforceID", paymentMethod.SalesforceID),
                    CreateParameter("@MethodName", paymentMethod.MethodName),
                    CreateParameter("@MethodType", paymentMethod.MethodType),
                    CreateParameter("@CurrentBalance", paymentMethod.CurrentBalance),
                    CreateParameter("@IsActive", paymentMethod.IsActive)
                };

                var result = db.ExecuteScalar("usp_SyncPaymentMethodFromSalesforce", parameters);
                return ToInt(result) > 0;
            }
        }

        /// <summary>
        /// Maps a DataRow to a PaymentMethod object
        /// </summary>
        private PaymentMethod MapDataRowToPaymentMethod(DataRow row)
        {
            return new PaymentMethod
            {
                PaymentMethodID = ToInt(row["PaymentMethodID"]),
                UserID = ToInt(row["UserID"]),
                MethodName = ToString(row["MethodName"]),
                MethodType = ToString(row["MethodType"]),
                CurrentBalance = ToNullableDecimal(row["CurrentBalance"]),
                IsActive = ToBoolean(row["IsActive"]),
                SalesforceID = ToString(row["SalesforceID"]),
                LastSyncDate = ToNullableDateTime(row["LastSyncDate"])
            };
        }
    }
}