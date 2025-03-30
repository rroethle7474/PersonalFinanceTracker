using System;
using System.Collections.Generic;

namespace PersonalFinanceTracker.Models
{
    /// <summary>
    /// Represents a user in the Personal Finance Tracker system
    /// </summary>
    public class User
    {
        /// <summary>
        /// User's unique identifier
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// User's login username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// User's email address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Hashed password for security
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// User's first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// User's last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Date when user account was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date of user's last login
        /// </summary>
        public DateTime? LastLoginDate { get; set; }
    }
}