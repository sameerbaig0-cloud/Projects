using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace ServicePlatform.Models
{
	[Table("POS_RetailArrivals")]
	public partial class POS_RetailArrivals
	{
		[Key]
		public long ArrivalID { get; set; }

		[Required]
		public DateTime ArrivalDate { get; set; }

		[Required]
		[StringLength(20)]
		public string OrderNo { get; set; }

		[Required]
		[StringLength(20)]
		public string Article { get; set; }

		[Required]
		[StringLength(6)]
		public string Size { get; set; }

		[Required]
		public int TrackingID { get; set; }

		[Required]
		public int Quantity { get; set; }

		[Required]
		public long StoreID { get; set; }

		[Required]
		[StringLength(450)]
		public string UserID { get; set; }

		[Required]
		public DateTime CreatedAt { get; set; }



		// Navigation properties
		[ForeignKey("StoreID")]
		public virtual POS_RetailStore POS_RetailStore { get; set; }

		[ForeignKey("UserID")]
		public virtual ApplicationUser? ApplicationUser { get; set; }  // Required reference navigation to principal
	}




	public class POS_RetailStoreArrivals
	{
		public Int64? ID { get; set; }

		[Required]
		[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = false)]
		[Display(Name = "Date")]
		public DateTime currentDate { get; set; }

		[Required]
		[Display(Name = "Scan Data")]
		public string qrCodeData { get; set; } = null!;

		[Required]
		[Display(Name = "Retail Store")]
		public long POSRetailStoreId { get; set; }

		public List<POS_RetailStore>? RetailStores { get; set; }
	}


}
