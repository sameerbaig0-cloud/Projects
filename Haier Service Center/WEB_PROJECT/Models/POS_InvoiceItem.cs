using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Permissions;

namespace ServicePlatform.Models
{

	[Table("POS_InvoiceItems", Schema = "Dbo")]
	public partial class POS_InvoiceItem
	{

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long InvoiceItemID { get; set; }

		[Required]
		public long InvoiceID { get; set; }

		[Required]
		[MaxLength(20)]
		public string ArticleNo { get; set; }

		[Required]
		[MaxLength(6)]
		public string Size { get; set; }

		[Required]
		public long TrackingID { get; set; }

		[Required]
		[DisplayName("Qty")]
		public int Quantity { get; set; } = 0;

		[Required]
		[DisplayName("Unit Price")]
		public decimal UnitPrice { get; set; } = 0;


		// Set default values for DiscountPercentage and TaxPercentage if needed		
		[Required]
		[DisplayName("Discount %")]
		[Range(0, 100, ErrorMessage = "Discount percentage must be between 0 and 100.")]
		public decimal DiscountPercentage { get; set; } = 0.0m;


		[Required]
		[DisplayName("Taxable Value")]
		public decimal TaxableValue { get; set; } = 0.0m;


		[Required]
		public long TaxID { get; set; }


		[Required]
		[DisplayName("Tax %")]
		[Range(0, 100, ErrorMessage = "Tax percentage must be between 0 and 100.")]
		public decimal TaxPercentage { get; set; } = 0.0m;


		[Required]
		[DisplayName("Total Price")]
		public decimal TotalPrice { get; set; } = 0.0m;






		[ForeignKey("InvoiceID")]
		public virtual POS_Invoice POS_Invoice { get; set; } = null!; // Required reference navigation to principal


		//[ForeignKey("ArticleNo")]
		//public virtual ArticleMaster ArticleMaster { get; set; } = null!; // Required reference navigation to principal


		[ForeignKey("TaxID")]
		public virtual POS_Taxation POS_Taxation { get; set; } = null!; // Required reference navigation to principal



		//public virtual ICollection<POS_Taxation> POS_Taxations { get; set; } = new List<POS_Taxation>();

		//public virtual ICollection<ArticleMaster> ArticleMasters { get; set; } = new List<ArticleMaster>();



	}



}
