using Humanizer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicePlatform.Models
{
	[Table("ApplicationName", Schema ="Dbo")]
	public partial class ApplicationName
	{

		public ApplicationName()
		{
			MenuItemRoleMappings = new List<MenuItemRoleMapping>();
			UserMenuAccesses = new List<UserMenuAccess>(); 
		}


		[Key]
		public long SNo { get; set; }
		public string AppId { get; set; }
		public string AppName { get; set; }



		// Collection navigation containing dependents
		public virtual ICollection<MenuItemRoleMapping> MenuItemRoleMappings { get; set; } = new List<MenuItemRoleMapping>();

		public virtual ICollection<UserMenuAccess> UserMenuAccesses { get; set; }	= new List<UserMenuAccess>();

	}

}

