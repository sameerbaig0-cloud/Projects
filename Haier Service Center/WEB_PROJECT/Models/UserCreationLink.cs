using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicePlatform.Models
{
	[Table("UserCreationLink", Schema ="identity")]
	public partial class UserCreationLink
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long CreationID { get; set; }

		[Display(Name ="Enter Email Id")]
		[Required]
        [EmailAddress]
        public string Email { get; set; }
		public Guid ActivationCode { get; set; }
		public string UserID { get; set; }
		public DateTime CreatedTime { get; set; }
		public bool IsActive { get; set; }


		[ForeignKey("UserID")]
		public virtual ApplicationUser ApplicationUser { get; set; } = null!;


	}



    public class UserCreationModel
    {

        [Display(Name = "Enter Email Id")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }

}
