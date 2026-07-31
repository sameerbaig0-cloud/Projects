
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicePlatform.Models
{
    [Table("Places", Schema = "dbo")]
    public class Location
    {
        public long SNo { get; set; }
        public string Place { get; set; }
        public string Zip { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string Circle { get; set; }
        public string Region { get; set; }
        public string Division { get; set; }
        public string OfficeType { get; set; }
        public string Delivery { get; set; }
        public string Country { get; set; }
        public string Lattitude { get; set; }
        public string Longitude { get; set; }

    }
}
