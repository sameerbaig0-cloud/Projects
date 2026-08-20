using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicePlatform.Models
{
	[Table("POS_Customers", Schema = "Dbo")]
	public partial class POS_Customers
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long CustomerID { get; set; }

		[Required]
		[MaxLength(50)]
		public string FirstName { get; set; }

		[Required]
		[MaxLength(50)]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Email is required.")]
		[MaxLength(100)]
		[EmailAddress(ErrorMessage = "Invalid email address.")]
		[Display(Name = "Email Address")]
		public string Email { get; set; }

		[MaxLength(20)]
		[Required(ErrorMessage = "Phone Number is required.")]
		public string Phone { get; set; }

		[MaxLength(255)]
		public string Address { get; set; }

		[MaxLength(100)]
		public string? City { get; set; }

		[MaxLength(50)]
		public string? State { get; set; }

		[MaxLength(20)]
		public string? ZipCode { get; set; }

		[MaxLength(100)]
		public string? Country { get; set; } = "India";

		public bool IsActive { get; set; }


		public virtual ICollection<POS_Invoice> POS_Invoices { get; set; } = new List<POS_Invoice>();


	}
}
