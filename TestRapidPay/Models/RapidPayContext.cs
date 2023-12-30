using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TestRapidPay.Models;

public partial class RapidPayContext : DbContext
{
    public RapidPayContext()
    {
    }

    public RapidPayContext(DbContextOptions<RapidPayContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Card> Cards { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\Local_DB;Initial Catalog=RapidPay;User ID=sa;Password=123456");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(e => e.CardId).HasName("PK__Cards__55FECDAE431BD2BA");

            entity.HasIndex(e => e.CardNumber, "UQ__Cards__A4E9FFE9B95E884A").IsUnique();

            entity.Property(e => e.Balance)
                .HasDefaultValueSql("((0.00))")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CardNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A38FE07006E");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Fee).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaymentDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            //entity.HasOne(d => d.Card).WithMany(p => p.Payments)
            //    .HasForeignKey(d => d.CardId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__Payments__CardId__619B8048");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CEA005EE7");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105342251657A").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
