using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServicePlatform.Models
{
	[Table("POS_InvoicesReturn", Schema = "Dbo")]
	public partial class POS_InvoicesReturn
	{

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long ReturnInvoiceID { get; set; }

		public long InvoiceID { get; set; }

		[Required]
		public DateTime ReturnDate { get; set; } = DateTime.Now;

		[Required]
		[Column(TypeName = "decimal(18, 2)")]
		public decimal TotalReturnAmount { get; set; }

		[Required]
		public DateTime CreatedTime { get; set; } = DateTime.Now;

		public long StoreID { get; set; }

		public string UserID { get; set; }


		[ForeignKey("InvoiceID")]
		public virtual POS_Invoice POS_Invoice { get; set; } = null!;

		[ForeignKey("StoreID")]
		public virtual POS_RetailStore POS_RetailStore { get; set; } = null!;

		[ForeignKey("UserID")]
		public virtual ApplicationUser User { get; set; } = null!;

		public ICollection<POS_InvoicesItemsReturn> POS_InvoicesItemsReturns { get; set; } = new List<POS_InvoicesItemsReturn>();

	}
}
