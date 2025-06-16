using System.ComponentModel.DataAnnotations;

namespace IzzyShop.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        // Vulnerable: Storing password in plain text
        [Required]
        public string Password { get; set; }

        // Vulnerable: Storing sensitive data without encryption
        public string CreditCardNumber { get; set; }
        public string SecurityCode { get; set; }
        public string ExpiryDate { get; set; }

        // Vulnerable: No proper validation on email
        public string Email { get; set; }

        // Vulnerable: No proper validation on phone number
        public string PhoneNumber { get; set; }

        // Vulnerable: No proper validation on address
        public string Address { get; set; }

        public bool IsAdmin { get; set; }

        // Vulnerable: No proper validation on session token
        public string SessionToken { get; set; }

        // Vulnerable: No proper validation on reset token
        public string PasswordResetToken { get; set; }
    }
} 