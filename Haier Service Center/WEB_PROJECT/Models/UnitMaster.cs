using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicePlatform.Models
{
    [Table("UnitMaster", Schema = "dbo")]
    public class UnitMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UnitID { get; set; }

        public required string UnitCode { get; set; }

        public required string UnitName { get; set; }
        public bool IsActive { get; set; }


        public virtual ICollection<ItemMaster> ItemMasters { get; set; } = new List<ItemMaster>();
    }
}