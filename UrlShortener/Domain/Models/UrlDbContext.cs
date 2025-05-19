using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace UrlShortener.Domain.Models;

public partial class UrlDbContext : DbContext
{
    public UrlDbContext()
    {
    }

    public UrlDbContext(DbContextOptions<UrlDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ShortUrlDatum> ShortUrlData { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=root;database=url_db", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.37-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<ShortUrlDatum>(entity =>
        {
            entity.HasKey(e => e.ShortUrl).HasName("PRIMARY");

            entity.ToTable("short_url_data");

            entity.Property(e => e.ShortUrl)
                .HasMaxLength(6)
                .HasColumnName("short_url");
            entity.Property(e => e.CreatedBy)
                .HasColumnType("text")
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.OriginalUrl)
                .HasMaxLength(1000)
                .HasColumnName("original_url");
            entity.Property(e => e.UpdatedBy)
                .HasColumnType("text")
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
