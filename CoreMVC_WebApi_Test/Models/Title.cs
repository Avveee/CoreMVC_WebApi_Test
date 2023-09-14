﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CoreMVC_WebApi_Test.Models;

[Table("titles")]
[Index("Title1", Name = "titleind")]
public partial class Title
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

    [Column("advance", TypeName = "money")]
    public decimal? Advance { get; set; }

    [Column("royalty")]
    public int? Royalty { get; set; }

    [Column("ytd_sales")]
    public int? YtdSales { get; set; }

    [Column("notes")]
    [StringLength(200)]
    [Unicode(false)]
    public string? Notes { get; set; }

    [Column("pubdate", TypeName = "datetime")]
    public DateTime Pubdate { get; set; }

    [ForeignKey("PubId")]
    //[InverseProperty("Titles")]
    public virtual Publisher? Pub { get; set; }
}
