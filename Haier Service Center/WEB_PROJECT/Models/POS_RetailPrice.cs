using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicePlatform.Models
{
	[Table("POS_RetailPrice", Schema = "Dbo")]

	public partial class POS_RetailPrice
	{

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long PriceID { get; set; }

		[Required]
		public long ItemID { get; set; }


		[Display(Name = "Price")]
		[Column(TypeName = "decimal(18,2)")]
		[Range(1, 100000, ErrorMessage = "Please enter the correct price")]
		public decimal Price { get; set; }


        [Display(Name = "HSN Code")]
        public long TaxID { get; set; }


		public DateTime? ModifiedDatetime { get; set; }

		public string? UserID { get; set; }

		public bool? IsActive { get; set; }



        //public virtual ICollection<POS_InvoiceItem> POS_InvoiceItems { get; set; } = new List<POS_InvoiceItem>();


        //[ForeignKey("ArticleNo")]
        //public virtual ArticleMaster? ArticleMaster { get; set; }  // Required reference navigation to principal

        [ForeignKey("ItemID")]
        public virtual POS_ItemMaster? POS_ItemMaster { get; set; }  // Required reference navigation to principal


        [ForeignKey("TaxID")]
		public virtual POS_Taxation? POS_Taxation { get; set; }  // Required reference navigation to principal


		[ForeignKey("UserID")]
		public virtual ApplicationUser? ApplicationUser { get; set; }  // Required reference navigation to principal


	}

}
