using CoreMVC_WebApi_Test.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CoreMVC_WebApi_Test.DTOs
{
    public class PublisherDTO
    {
        [Key]
        [Column("pub_id")]
        [StringLength(4)]
        [Unicode(false)]
        public string PubId { get; set; } = null!;

        [Column("pub_name")]
        [StringLength(40)]
        [Unicode(false)]
        public string? PubName { get; set; }

        [Column("city")]
        [StringLength(20)]
        [Unicode(false)]
        public string? City { get; set; }

        [Column("state")]
        [StringLength(2)]
        [Unicode(false)]
        public string? State { get; set; }

        [Column("country")]
        [StringLength(30)]
        [Unicode(false)]
        public string? Country { get; set; }
    }
}
