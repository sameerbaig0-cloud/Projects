using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServicePlatform.Models
{
	[Table("CompanyInfo")]
	public partial class CompanyInfo
	{
		[MaxLength(100)]
		public string CompanyName { get; set; }

		[MaxLength(100)]
		public string SubName { get; set; }

		[MaxLength(100)]
		public string Address1 { get; set; }

		[MaxLength(100)]
		public string Address2 { get; set; }

		[MaxLength(100)]
		public string Address3 { get; set; }

		[MaxLength(50)]
		public string City { get; set; }

		[MaxLength(10)]
		public string PinCode { get; set; }

		[MaxLength(25)]
		public string District { get; set; }

		[MaxLength(25)]
		public string State { get; set; }

		[MaxLength(50)]
		public string Country { get; set; }

		[MaxLength(50)]
		public string ContactPerson { get; set; }

		[MaxLength(15)]
		public string Phone { get; set; }

		[MaxLength(15)]
		public string Fax { get; set; }

		[MaxLength(50)]
		public string Email { get; set; }

		[MaxLength(15)]
		public string GSTIN { get; set; }

		[MaxLength(15)]
		public string TINNo { get; set; }

		[MaxLength(25)]
		public string CSTNo { get; set; }

		[MaxLength(15)]
		public string IECode { get; set; }

		[MaxLength(15)]
		public string WareHouseCode { get; set; }

		[MaxLength(15)]
		public string PBWHLicenseNo { get; set; }

		[MaxLength(10)]
		public string PANNo { get; set; }

		[MaxLength(25)]
		public string CINNo { get; set; }

		[MaxLength(30)]
		public string LUT { get; set; }

		public byte[] CompanyLogo { get; set; }
	}
}
