using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicePlatform.Models
{

	[Table("POS_PackingToRetailTransfers", Schema = "Dbo")]
	public partial class POS_PackingToRetailTransfer
	{

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long TransferID { get; set; }

		[Required]
		[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = false)]
		[Display(Name = "Date")]
		public DateTime TransferDate { get; set; }

		[Required]
		[Display(Name = "Order No")]
		public string OrderNo { get; set; }

		[Required]
		[Display(Name = "Article")]
		public string Article { get; set; }

		[Required]
		[Display(Name = "Size")]
		public string Size { get; set; }

		[Required]
		[Display(Name = "Tracking ID")]
		public int TrackingID { get; set; }

		[Required]
		[Display(Name = "Quantity")]
		public int? Quantity { get; set; }

        [Required]
        [Display(Name = "Store")]
        public long StoreID { get; set; }

		[Required]
		public string UserID { get; set; } = null!;

		public DateTime CreatedAt { get; set; }




		[ForeignKey("UserID")]
		public virtual ApplicationUser ApplicationUser { get; set; } = null!; // Required reference navigation to principal


		[ForeignKey("StoreID")]
		public virtual POS_RetailStore POS_RetailStore { get; set; } = null!; // Required reference navigation to principal



	}




	public class POS_PackingToRetailStoreTransfer
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



