using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicePlatform.Models
{
    [Table("ClickEventAuthorization", Schema = "dbo")]
    public partial class ClickEventAuthorization
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string ControllerName { get; set; }

        [Required]
        public string ActionName { get; set; }



        [ForeignKey("UserId")]
        public virtual ApplicationUser? ApplicationUser { get; set; } = null!; // Required reference navigation to principal



    }
}
