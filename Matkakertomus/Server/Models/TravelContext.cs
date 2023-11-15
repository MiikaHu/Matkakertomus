using Microsoft.EntityFrameworkCore;

namespace Matkakertomus.Server.Models;

public partial class TravelContext : DbContext
{
    public TravelContext()
    {
    }

    public TravelContext(DbContextOptions<TravelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Picture> Pictures { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    public virtual DbSet<Traveller> Travellers { get; set; }

    public virtual DbSet<Destination> Destinations { get; set; }

    public virtual DbSet<Story> Stories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite("Data Source=data.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Picture>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PRIMARY");

            entity.ToTable("kuva");

            entity.HasIndex(e => e.StoryId, "fk_kuva_tarina1_idx");

            entity.Property(e => e.ImageId).HasColumnName("idkuva");
            entity.Property(e => e.StoryId).HasColumnName("idtarina");
            entity.Property(e => e.Image)
                .HasMaxLength(45)
                .HasColumnName("kuva");

            entity.HasOne(d => d.Story).WithMany(p => p.Pictures)
                .HasForeignKey(d => d.StoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_kuva_tarina1");
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(e => e.TripId).HasName("PRIMARY");

            entity.ToTable("matka");

            entity.HasIndex(e => e.TravellerId, "fk_matkaaja_has_matkakohde_matkaaja_idx");

            entity.Property(e => e.TripId).HasColumnName("idmatka");
            entity.Property(e => e.StartDate)
                .HasColumnType("date")
                .HasColumnName("alkupvm");
            entity.Property(e => e.TravellerId).HasColumnName("idmatkaaja");
            entity.Property(e => e.EndDate)
                .HasColumnType("date")
                .HasColumnName("loppupvm");
            entity.Property(e => e.Private).HasColumnName("yksityinen");

            entity.HasOne(d => d.Traveller).WithMany(p => p.Trips)
                .HasForeignKey(d => d.TravellerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_matkaaja_has_matkakohde_matkaaja");
        });

        modelBuilder.Entity<Traveller>(entity =>
        {
            entity.HasKey(e => e.TravellerId).HasName("PRIMARY");

            entity.ToTable("matkaaja");

            entity.Property(e => e.TravellerId).HasColumnName("idmatkaaja");
            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .HasColumnName("email");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("esittely");
            entity.Property(e => e.FirstName)
                .HasMaxLength(45)
                .HasColumnName("etunimi");
            entity.Property(e => e.Image)
                .HasMaxLength(45)
                .HasColumnName("kuva");
            entity.Property(e => e.Username)
                .HasMaxLength(45)
                .HasColumnName("nimimerkki");
            entity.Property(e => e.Area)
                .HasMaxLength(45)
                .HasColumnName("paikkakunta");
            entity.Property(e => e.PasswordHash)
                .HasColumnName("passwordhash");
            entity.Property(e => e.PasswordSalt)
    .HasColumnName("passwordSalt");
            entity.Property(e => e.LastName)
                .HasMaxLength(45)
                .HasColumnName("sukunimi");
        });

        modelBuilder.Entity<Destination>(entity =>
        {
            entity.HasKey(e => e.DestinationId).HasName("PRIMARY");

            entity.ToTable("matkakohde");

            entity.Property(e => e.DestinationId).HasColumnName("idmatkakohde");
            entity.Property(e => e.DestinationName)
                .HasMaxLength(45)
                .HasColumnName("kohdenimi");
            entity.Property(e => e.Image)
                .HasMaxLength(45)
                .HasColumnName("kuva");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("kuvausteksti");
            entity.Property(e => e.Country)
                .HasMaxLength(45)
                .HasColumnName("maa");
            entity.Property(e => e.Area)
                .HasMaxLength(45)
                .HasColumnName("paikkakunta");
        });

        modelBuilder.Entity<Story>(entity =>
        {
            entity.HasKey(e => e.StoryId).HasName("PRIMARY");

            entity.ToTable("tarina");

            entity.HasIndex(e => e.StoryId, "fk_tarina_matka1_idx");

            entity.HasIndex(e => e.DestinationId, "fk_tarina_matkakohde1_idx");

            entity.Property(e => e.StoryId).HasColumnName("idtarina");
            entity.Property(e => e.TripId).HasColumnName("idmatka");
            entity.Property(e => e.DestinationId).HasColumnName("idmatkakohde");
            entity.Property(e => e.Date)
                .HasColumnType("date")
                .HasColumnName("pvm");
            entity.Property(e => e.Text)
                .HasMaxLength(500)
                .HasColumnName("teksti");

            entity.HasOne(d => d.Trip).WithMany(p => p.Stories)
                .HasForeignKey(d => d.TripId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_tarina_matka1");

            entity.HasOne(d => d.Destination).WithMany(p => p.Stories)
                .HasForeignKey(d => d.DestinationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tarina_matkakohde1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
