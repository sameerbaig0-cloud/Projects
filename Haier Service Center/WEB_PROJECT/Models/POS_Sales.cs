using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicePlatform.Models
{
	public class POS_Sales
	{
		[Key]
		public int Id { get; set; }


		public POS_Customers Customer { get; set; }
		public POS_PaymentType PaymentType { get; set; }
		public POS_Invoice Invoice { get; set; }
		public List<POS_InvoiceItem> InvoiceItems { get; set; }


		// Include these properties directly
		[Display(Name = "Sub Total")]
		public decimal SubTotal { get; set; }

		[Display(Name = "Discount %")]
		[Range(0, 100, ErrorMessage = "Discount percentage must be between 0 and 100.")]
		public decimal DiscountPercentage { get; set; }

		//[Display(Name = "Tax %")]
		//[Range(0, 100, ErrorMessage = "Tax percentage must be between 0 and 100.")]
		//public decimal TaxPercentage { get; set; }
		[Display(Name = "Net Amount")]
		public decimal NetTotal { get; set; }


		public POS_Sales()
		{
			Customer = new POS_Customers();
			PaymentType = new POS_PaymentType();
			Invoice = new POS_Invoice();
			InvoiceItems = new List<POS_InvoiceItem>();
		}


	}


	//VIEW TABLE MODEL
	[Table("POS_SalesScanningVw")]
	public partial class POS_SalesScanningSummary
	{
		[Key]
		public long ID { get; set; }
		public long CustomerID { get; set; }
		public string Article { get; set; }
		public string ArticleDescription { get; set; }
		public string ArticleBrand { get; set; }
		public string Size { get; set; }
		public int Quantity { get; set; }
		public decimal UnitPrice { get; set; }
		public decimal Total { get; set; }
		public decimal CGSTPercentage { get; set; }
		public decimal SGSTPercentage { get; set; }
		public decimal TaxableValue { get; set; }
		public long TrackingID { get; set; }
	}


}
