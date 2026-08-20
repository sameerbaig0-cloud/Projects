using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicePlatform.Models
{
    [Table("ShowRooms", Schema = "dbo")]
    public class ShowRoom
    {
        [Key]
        [Display(Name = "Show Room ID")]
        public int ShowRoomId { get; set; }

        [Required(ErrorMessage = "Show room name is required.")]
        [StringLength(150, ErrorMessage = "Name cannot exceed 150 characters.")]
        [Display(Name = "Show Room Name")]
        public string Name { get; set; }

        [StringLength(300, ErrorMessage = "Address cannot exceed 300 characters.")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters.")]
        [Display(Name = "City")]
        public string City { get; set; }

        [StringLength(10, ErrorMessage = "Zip cannot exceed 10 characters.")]
        [Display(Name = "ZipCode")]
        public string ZipCode { get; set; }

        [StringLength(100, ErrorMessage = "District cannot exceed 100 characters.")]
        [Display(Name = "District")]
        public string District { get; set; }

        [StringLength(100, ErrorMessage = "State cannot exceed 100 characters.")]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters.")]
        [Display(Name = "Phone Number")]
        public string? Phone { get; set; }

        [StringLength(20, ErrorMessage = "Alternate phone cannot exceed 20 characters.")]
        [Display(Name = "Alternate Phone")]
        public string? AlternatePhone { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(150, ErrorMessage = "Email cannot exceed 150 characters.")]
        [Display(Name = "Email Address")]
        public string? Email { get; set; }

        [StringLength(15, ErrorMessage = "GST number cannot exceed 15 characters.")]
        [Display(Name = "GST Number")]
        public string? GSTNumber { get; set; }

        [StringLength(150, ErrorMessage = "Contact person name cannot exceed 150 characters.")]
        [Display(Name = "Contact Person")]
        public string? ContactPerson { get; set; }

        [Display(Name = "Latitude")]
        public decimal? Latitude { get; set; }

        [Display(Name = "Longitude")]
        public decimal? Longitude { get; set; }

        [StringLength(100, ErrorMessage = "Google Place ID cannot exceed 100 characters.")]
        [Display(Name = "Google Place ID")]
        public string? GooglePlaceId { get; set; }  

        [Display(Name = "Opening Time")]
        public TimeSpan? OpeningTime { get; set; }

        [Display(Name = "Closing Time")]
        public TimeSpan? ClosingTime { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Is Deleted")]
        public bool IsDeleted { get; set; } = false;

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Created By")]
        public string? CreatedBy { get; set; }

        [Display(Name = "Updated Date")]
        public DateTime? UpdatedDate { get; set; }

        [Display(Name = "Updated By")]
        public string? UpdatedBy { get; set; }
    }

}
