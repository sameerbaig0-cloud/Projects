using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicePlatform.Models
{
    [Table("MenuItemRoleMapping", Schema = "dbo")]
    public partial class MenuItemRoleMapping
    {
        [Key]
        public int Id { get; set; }

        public int MenuItemId { get; set; }
        public long ApplicationNameId { get; set; }


        [ForeignKey("MenuItemId")]
        public virtual MenuItem MenuItem { get; set; } = null!; // Required reference navigation to principal

        [ForeignKey("ApplicationNameId")]
        public virtual ApplicationName ApplicationName { get; set; } = null!; // Required reference navigation to principal


    }
}
