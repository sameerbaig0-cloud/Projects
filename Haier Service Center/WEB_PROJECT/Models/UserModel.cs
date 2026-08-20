using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServicePlatform.Models
{
    [Table("User")]
    public class UserModel
    {
            [Key]
            [MaxLength(450)]
            public string Id { get; set; }

            [MaxLength(256)]
            public string UserName { get; set; }

            [MaxLength(256)]
            public string NormalizedUserName { get; set; }

            [MaxLength(256)]
            public string Email { get; set; }

            [MaxLength(256)]
            public string NormalizedEmail { get; set; }

            public bool EmailConfirmed { get; set; }

            [MaxLength]
            public string PasswordHash { get; set; }

            [MaxLength]
            public string SecurityStamp { get; set; }

            [MaxLength]
            public string ConcurrencyStamp { get; set; }

            [MaxLength]
            public string PhoneNumber { get; set; }

            public bool PhoneNumberConfirmed { get; set; }

            public bool TwoFactorEnabled { get; set; }

            public DateTimeOffset? LockoutEnd { get; set; }

            public bool LockoutEnabled { get; set; }

            public int AccessFailedCount { get; set; }

            [MaxLength]
            public string FirstName { get; set; }

            [MaxLength]
            public string LastName { get; set; }

            [Column(TypeName = "varbinary(max)")]
            public byte[] ProfilePicture { get; set; }

            public int UsernameChangeLimit { get; set; }

            public bool? IsDeactivated { get; set; }

            public DateTime? DeactivationDateTime { get; set; }

            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int AccountID { get; set; }

    }
}
