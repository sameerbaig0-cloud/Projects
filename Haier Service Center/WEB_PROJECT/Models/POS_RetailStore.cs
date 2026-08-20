using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServicePlatform.Models
{
	[Table("POS_RetailStore")]
	public partial class POS_RetailStore
	{

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long StoreID { get; set; }

		[MaxLength(200)]
		public string StoreName { get; set; }

		[MaxLength(200)]
		public string StoreAddress1 { get; set; }

		[MaxLength(200)]
		public string StoreAddress2 { get; set; }

		[MaxLength(25)]
		public string StoreCity { get; set; }

		[MaxLength(10)]
		public string StorePinCode { get; set; }

		[MaxLength(25)]
		public string StoreDistrict { get; set; }

		[MaxLength(25)]
		public string StoreState { get; set; }

		[MaxLength(25)]
		public string StoreCountry { get; set; }

		[MaxLength(25)]
		public string StoreLicenseNo { get; set; }

		public bool? StoreFlag { get; set; }



		public virtual ICollection<POS_Invoice> POS_Invoices { get; set; } = new List<POS_Invoice>();
		public virtual ICollection<POS_PackingToRetailTransfer> POS_PackingToRetailTransfers { get; set; } = new List<POS_PackingToRetailTransfer>();


	}
}
