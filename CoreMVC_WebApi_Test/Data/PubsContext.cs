using System;
using System.Collections.Generic;
using CoreMVC_WebApi_Test.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoreMVC_WebApi_Test.Data;

public partial class PubsContext : IdentityDbContext<ApplicationUser>
{
    public PubsContext(DbContextOptions<PubsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<Title> Titles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.PubId).HasName("UPKCL_pubind");

            entity.Property(e => e.PubId).IsFixedLength();
            entity.Property(e => e.Country).HasDefaultValueSql("('USA')");
            entity.Property(e => e.State).IsFixedLength();
        });

        modelBuilder.Entity<Title>(entity =>
        {
            entity.HasKey(e => e.TitleId).HasName("UPKCL_titleidind");

            entity.Property(e => e.PubId).IsFixedLength();
            entity.Property(e => e.Pubdate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Type)
                .HasDefaultValueSql("('UNDECIDED')")
                .IsFixedLength();

            entity.HasOne(d => d.Pub).WithMany(p => p.Titles).HasConstraintName("FK__titles__pub_id__014935CB");
        });

        OnModelCreatingPartial(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
