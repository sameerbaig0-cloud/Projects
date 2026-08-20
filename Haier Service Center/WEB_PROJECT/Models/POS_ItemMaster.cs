using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicePlatform.Models
{
    [Table("POS_ItemMaster", Schema = "dbo")]
    public partial class POS_ItemMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ItemID { get; set; }

        public required string ItemCode { get; set; }

        public required string ItemName { get; set; }

        public long UnitID { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public bool IsActive { get; set; }




        [ForeignKey("UnitID")]
        public virtual UnitMaster? UnitMaster { get; set; } = null!; // Required reference navigation to principal


        public virtual ICollection<POS_RetailPrice> POS_RetailPrices { get; set; } = new List<POS_RetailPrice>();

    }
}
