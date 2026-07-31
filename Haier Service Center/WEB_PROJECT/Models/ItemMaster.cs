using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicePlatform.Models
{
    [Table("ItemMaster", Schema = "dbo")]
    public partial class ItemMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ItemID { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public long UnitID { get; set; }

        public string CreatedDateTime { get; set; }

        public bool IsActive { get; set; }

    }
}
