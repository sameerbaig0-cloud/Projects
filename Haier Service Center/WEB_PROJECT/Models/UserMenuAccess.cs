using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicePlatform.Models
{
	[Table("UserMenuAccess", Schema ="Identity")]
	public partial class UserMenuAccess
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long ID { get; set; }

		[Required]
		[StringLength(450)]
		public string UserID { get; set; }

		[Required]
		public long ApplicationNameId { get; set; }
		public bool Flag { get; set; }


		[ForeignKey("UserID")]
		public virtual ApplicationUser? ApplicationUser { get; set; } = null!;


		[ForeignKey("ApplicationNameId")]
		public virtual ApplicationName?	ApplicationName { get; set; } = null!;


	}

}
