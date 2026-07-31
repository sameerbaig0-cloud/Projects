using System.ComponentModel.DataAnnotations;

namespace ServicePlatform.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Please enter your name.")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your message.")]
        [StringLength(500)]
        public string Message { get; set; }
    }

}
