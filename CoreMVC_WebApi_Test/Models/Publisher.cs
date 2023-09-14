using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CoreMVC_WebApi_Test.Models;

[Table("publishers")]
public partial class Publisher
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

    //[InverseProperty("Pub")]
    public virtual ICollection<Title> Titles { get; set; } = new List<Title>();
}
