using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CoreMVC_WebApi_Test.DTO_s
{
    public class TitleDTO
    {
        [Key]
        [Column("title_id")]
        [StringLength(6)]
        [Unicode(false)]
        public string TitleId { get; set; } = null!;

        [Column("title")]
        [StringLength(80)]
        [Unicode(false)]
        public string? Title1 { get; set; } = null!;

        [Column("type")]
        [StringLength(12)]
        [Unicode(false)]
        public string Type { get; set; } = null!;

        [Column("pub_id")]
        [StringLength(4)]
        public string? PubId { get; set; }

        [Column("price", TypeName = "money")]
        public decimal? Price { get; set; }

        [Column("pubdate", TypeName = "datetime")]
        public DateTime Pubdate { get; set; }
    }
}
