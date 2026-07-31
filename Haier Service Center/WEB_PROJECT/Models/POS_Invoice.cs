using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicePlatform.Models
{
	[Table("POS_Invoices", Schema = "Dbo")]
	public partial class POS_Invoice
	{


		[Key]
		//[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Display(Name = "Invoice No")]
		public long InvoiceID { get; set; }

		[Required]
		public long CustomerID { get; set; }

		[Required]
		[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = false)]
		[Display(Name = "Date")]
		public DateTime InvoiceDate { get; set; }

		[Required]
		public DateTime DueDate { get; set; }

		[Required]
		[Display(Name = "Invoice Value")]
		public decimal TotalAmount { get; set; }

		[Required]
		[Display(Name = "Payment Mode")]
		public int PaymentTypeID { get; set; }

		[Required]
		[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = false)]
		[Display(Name = "Time")]
		public DateTime CreatedTime { get; set; }

		[Required]
		public bool IsPaid { get; set; }

		[Required]
		public long StoreID { get; set; }

		[Required]
		public string UserID { get; set; }




		[ForeignKey("CustomerID")]
		public virtual POS_Customers POS_Customer { get; set; } = null!; // Required reference navigation to principal


		[ForeignKey("PaymentTypeID")]
		public virtual POS_PaymentType POS_PaymentType { get; set; } = null!; // Required reference navigation to principal


		[ForeignKey("UserID")]
		public virtual ApplicationUser ApplicationUser { get; set; } = null!; // Required reference navigation to principal


		[ForeignKey("StoreID")]
		public virtual POS_RetailStore POS_RetailStore { get; set; } = null!; // Required reference navigation to principal




		//public virtual ICollection<POS_RetailStore> POS_RetailStores { get; set; } = new List<POS_RetailStore>();

		public virtual ICollection<POS_InvoiceItem> POS_InvoiceItems { get; set; } = new List<POS_InvoiceItem>();


	}

}
