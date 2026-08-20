using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicePlatform.Models
{
    [Table("MenuItems", Schema ="dbo")]
    public partial class MenuItem
    {

        public MenuItem()
        {
            MenuItems = new HashSet<MenuItem>();
			UserPageAuthorizations = new HashSet<UserPageAuthorization>();
		}


        [Key]
        public int Id { get; set; }

        public string Title { get; set; } = null!;
        public long MenuSorting { get; set; }
        public string Url { get; set; } = null!;    
        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public List<MenuItem>? Submenu { get; set; }


        // Collection navigation containing dependents
        public virtual ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        public virtual ICollection<MenuItemRoleMapping> MenuItemRoleMappings { get; set; } = new List<MenuItemRoleMapping>();
        public virtual ICollection<UserPageAuthorization> UserPageAuthorizations { get; set;} = new List<UserPageAuthorization>();


    }
}

