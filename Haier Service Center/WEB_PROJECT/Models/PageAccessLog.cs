using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicePlatform.Models
{
    [Table(name: "PageAccessLog")]
    public partial class PageAccessLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LogId { get; set; }
        public string UserId { get; set; }
        public string PagePath { get; set; }
        public DateTime AccessTime { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string IpAddress { get; set; }


        public ApplicationUser ApplicationUser { get; set; }


    }
}
