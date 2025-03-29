using PersonalFinanceTracker.Models;
using System.Data;

namespace PersonalFinanceTracker.Data.Repositories
{
    /// <summary>
    /// Repository for User data operations
    /// </summary>
    public class UserRepository : BaseRepository, IUserRepository
    {
        /// <summary>
        /// Creates a new instance of UserRepository
        /// </summary>
        public UserRepository(IDatabaseFactory dbFactory) : base(dbFactory)
        {
        }

        /// <summary>
        /// Gets a user by ID
        /// </summary>
        public User GetById(int userId)
        {
            using (var db = CreateContext())
            {
                var param = CreateParameter("@UserID", userId);
                var dt = db.ExecuteStoredProcedure("usp_GetUserById", param);

                if (dt.Rows.Count == 0)
                    return null;

                return MapDataRowToUser(dt.Rows[0]);
            }
        }

        /// <summary>
        /// Gets a user by username
        /// </summary>
        public User GetByUsername(string username)
        {
            using (var db = CreateContext())
            {
                var param = CreateParameter("@Username", username);
                var dt = db.ExecuteStoredProcedure("usp_GetUserByUsername", param);

                if (dt.Rows.Count == 0)
                    return null;

                return MapDataRowToUser(dt.Rows[0]);
            }
        }

        /// <summary>
        /// Gets a user by email address
        /// </summary>
        public User GetByEmail(string email)
        {
            using (var db = CreateContext())
            {
                var param = CreateParameter("@Email", email);
                var dt = db.ExecuteStoredProcedure("usp_GetUserByEmail", param);

                if (dt.Rows.Count == 0)
                    return null;

                return MapDataRowToUser(dt.Rows[0]);
            }
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        public int Create(User user, string passwordHash)
        {
            using (var db = CreateContext())
            {
                var parameters = new[]
                {
                    CreateParameter("@Username", user.Username),
                    CreateParameter("@Email", user.Email),
                    CreateParameter("@PasswordHash", passwordHash),
                    CreateParameter("@FirstName", user.FirstName),
                    CreateParameter("@LastName", user.LastName)
                };

                var result = db.ExecuteScalar("usp_CreateUser", parameters);
                return ToInt(result);
            }
        }

        /// <summary>
        /// Updates an existing user
        /// </summary>
        public bool Update(User user)
        {
            using (var db = CreateContext())
            {
                var parameters = new[]
                {
                    CreateParameter("@UserID", user.UserID),
                    CreateParameter("@FirstName", user.FirstName),
                    CreateParameter("@LastName", user.LastName),
                    CreateParameter("@Email", user.Email)
                };

                return db.ExecuteNonQuery("usp_UpdateUser", parameters) > 0;
            }
        }

        /// <summary>
        /// Authenticates a user
        /// </summary>
        public bool Authenticate(string username, string passwordHash)
        {
            using (var db = CreateContext())
            {
                var parameters = new[]
                {
                    CreateParameter("@Username", username),
                    CreateParameter("@PasswordHash", passwordHash)
                };

                var dt = db.ExecuteStoredProcedure("usp_AuthenticateUser", parameters);
                return dt.Rows.Count > 0;
            }
        }

        /// <summary>
        /// Maps a DataRow to a User object
        /// </summary>
        private User MapDataRowToUser(DataRow row)
        {
            return new User
            {
                UserID = ToInt(row["UserID"]),
                Username = ToString(row["Username"]),
                Email = ToString(row["Email"]),
                FirstName = ToString(row["FirstName"]),
                LastName = ToString(row["LastName"]),
                CreatedDate = ToDateTime(row["CreatedDate"]),
                LastLoginDate = ToNullableDateTime(row["LastLoginDate"])
            };
        }
    }
}