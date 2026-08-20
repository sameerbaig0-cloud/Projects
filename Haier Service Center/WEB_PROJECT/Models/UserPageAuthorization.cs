using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicePlatform.Models
{
	[Table("UserPageAuthorization", Schema = "dbo")]
	public partial class UserPageAuthorization
	{
		//public UserPageAuthorization()
		//{
		//	ApplicationUsers = new HashSet<ApplicationUser>();
		//	MenuItems = new HashSet<MenuItem>();
		//}


		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public string? ID { get; set; } // Assuming ID is the primary key
		
		[DisplayName("Email")]
		public string UserID { get; set; } = null!;

		[DisplayName("Menu")]
		public int MenuItemsID { get; set; }

		[DisplayName("Is Authorized")]
		public bool IsAuthorized { get; set; }



		// Collection navigation containing dependents
		//public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();

		//public virtual ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();



		[ForeignKey("UserID")]
		public virtual ApplicationUser? ApplicationUser { get; set; } = null!; // Required reference navigation to principal

		[ForeignKey("MenuItemsID")]
		public virtual MenuItem? MenuItem { get; set; } = null!; // Required reference navigation to principal



	}

}
