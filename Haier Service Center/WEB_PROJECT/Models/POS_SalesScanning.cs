using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicePlatform.Models
{
	[Table("POS_SalesScanning")]
	public partial class POS_SalesScanning
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long ID { get; set; }

		[ForeignKey("POS_Customers")]
		public long? CustomerID { get; set; }

		[Column(TypeName = "smalldatetime")]
		public DateTime? InvoiceDate { get; set; }

		[Column(TypeName = "smalldatetime")]
		public DateTime? DueDate { get; set; }

		[StringLength(50)]
		public string ArticleNo { get; set; }

		[StringLength(6)]
		public string Size { get; set; }

		public long? TrackingID { get; set; }

		public int? Quantity { get; set; }

		[Column(TypeName = "decimal(18, 2)")]
		public decimal? UnitPrice { get; set; }


		public long TaxID { get; set; }



		[ForeignKey("TaxID")]
		public virtual POS_Taxation? POS_Taxation { get; set; } = null!; // Required reference navigation to principal


		public virtual POS_Customers POS_Customers { get; set; }


	}
}
