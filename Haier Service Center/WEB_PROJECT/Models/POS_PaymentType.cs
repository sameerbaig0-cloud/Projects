using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;


namespace ServicePlatform.Models
{
	[Table("POS_PaymentTypes", Schema="Dbo")]
	public partial class POS_PaymentType
	{
		[Key]
		//[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int PaymentTypeID { get; set; }

		[Required]
		[MaxLength(50)]
		[DisplayName("Payment Type")]
		public string Name { get; set; }

		public string Description { get; set; }

		[Required]
		public bool IsActive { get; set; }




		// Navigation property for invoices
		public virtual ICollection<POS_Invoice> POS_Invoices { get; set; } = new List<POS_Invoice>();

	}

}
