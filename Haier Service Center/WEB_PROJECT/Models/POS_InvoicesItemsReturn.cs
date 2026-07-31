using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServicePlatform.Models
{
	[Table("POS_InvoicesItemsReturn", Schema ="Dbo")]
	public partial class POS_InvoicesItemsReturn
	{

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long ReturnInvoiceItemID { get; set; }

		public long ReturnInvoiceID { get; set; }

		public long InvoiceItemID { get; set; }

		[Required]
		public int ReturnQuantity { get; set; }

		[Required]
		[Column(TypeName = "decimal(18, 2)")]
		public decimal ReturnUnitPrice { get; set; }

		[Required]
		[Column(TypeName = "decimal(18, 2)")]
		public decimal ReturnTotalPrice { get; set; }


		[ForeignKey("ReturnInvoiceID")]
		public virtual POS_InvoicesReturn POS_InvoicesReturn { get; set; } = null!;

		[ForeignKey("InvoiceItemID")]
		public virtual POS_InvoiceItem POS_InvoiceItem { get; set; } = null!;

	}
}
