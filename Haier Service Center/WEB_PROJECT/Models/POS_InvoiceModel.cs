using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicePlatform.Models
{
	public class POS_InvoiceModel 
	{

		[Key]
		public long POS_InvoiceID { get; set; }

		[NotMapped]
		public CompanyInfo? CompanyInfo { get; set; }

		public POS_RetailStore? POS_RetailStore { get; set; }
		public POS_Invoice? POS_Invoice { get; set; }
		public POS_Customers? POS_Customer { get; set; }
		public List<POS_InvoiceItem>? POS_InvoiceItem { get; set; }
		public POS_PaymentType? POS_PaymentType { get; set; }

		//public POS_Taxation? POS_Taxation { get; set; }
		public List<TaxationSummaryViewModel>? TaxationSummaryViewModels { get; set; }

		public ApplicationUser? ApplicationUser { get; set; }
		//public ArticleMaster? ArticleMaster { get; set; }
		//public ArticleBrandModel? ArticleBrand { get; set; }
		public List<string>? AdditionalInformation { get; set; }
		public List<string>? Acknowledgements { get; set; }



		public string? AmountInWords { get; set; }

		public decimal? SubTotalValue { get; set; }
		public decimal? DiscountValue { get; set; }


	}


	public class TaxationSummaryViewModel
	{
		public string HSNCode { get; set; }
		public string HSNDesc { get; set; }
		public decimal TaxableValue { get; set; }
		public decimal CGSTPercent { get; set; }
		public decimal CGSTValue { get; set; }
		public decimal SGSTPercent { get; set; }
		public decimal SGSTValue { get; set; }
	}

}
