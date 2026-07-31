using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicePlatform.Models
{
	[Table("POS_Taxation", Schema = "Dbo")]
	public partial class POS_Taxation
	{

		public POS_Taxation()
		{

			//POS_RetailPrices = new HashSet<POS_RetailPrice>();
			POS_InvoiceItems = new HashSet<POS_InvoiceItem>();
			//POS_SalesScannings = new HashSet<POS_SalesScanning>();
		}


		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long TaxID { get; set; }


		[Required]
		[StringLength(12)]
		public string HSNCode { get; set; } = null!;


		[Required]
		[StringLength(200)]
		public string HSNDescription { get; set; } = null!;


		[Required]
		[Column(TypeName = "decimal(5,2)")]
		public decimal CGSTPercentage { get; set; }


		[Required]
		[Column(TypeName = "decimal(5,2)")]
		public decimal SGSTPercentage { get; set; }


		[Required]
		[Column(TypeName = "decimal(5,2)")]
		public decimal IGSTPercentage { get; set; }


		[Required]
		public DateTime ModifiedDateTime {  get; set; }

		public Boolean Flag { get; set; }	




		public virtual ICollection<POS_RetailPrice> POS_RetailPrices { get; set; } = new List<POS_RetailPrice>();

		public virtual ICollection<POS_InvoiceItem> POS_InvoiceItems { get; set; } = new List<POS_InvoiceItem>();

		//public virtual ICollection<POS_SalesScanning> POS_SalesScannings { get; set; } = new List<POS_SalesScanning>();



	}
}
