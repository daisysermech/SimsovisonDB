using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SimsovisionDataBase
{
    public partial class SimsovisionDBContext : DbContext
    {
        public SimsovisionDBContext()
        {
        }

        public SimsovisionDBContext(DbContextOptions<SimsovisionDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<Nominations> Nominations { get; set; }
        public virtual DbSet<ParticipantTypes> ParticipantTypes { get; set; }
        public virtual DbSet<Participants> Participants { get; set; }
        public virtual DbSet<Participations> Participations { get; set; }
        public virtual DbSet<Songs> Songs { get; set; }
        public virtual DbSet<Years> Years { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=COMPUTER; Database=SimsovisionDB; Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cities>(entity =>
            {
                entity.HasKey(e => e.IdCity);

                entity.Property(e => e.IdCity).HasColumnName("ID_City");

                entity.Property(e => e.CityName)
                    .IsRequired()
                    .HasColumnName("City_Name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description).HasColumnType("text");
            });

            modelBuilder.Entity<Nominations>(entity =>
            {
                entity.HasKey(e => e.IdNomination);

                entity.Property(e => e.IdNomination).HasColumnName("ID_Nomination");

                entity.Property(e => e.NominationName)
                    .IsRequired()
                    .HasColumnName("Nomination_Name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ParticipantTypes>(entity =>
            {
                entity.HasKey(e => e.IdParticipantType);

                entity.Property(e => e.IdParticipantType).HasColumnName("ID_ParticipantType");

                entity.Property(e => e.ParticipantType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Participants>(entity =>
            {
                entity.HasKey(e => e.IdParticipant);

                entity.Property(e => e.IdParticipant).HasColumnName("ID_Participant");

                entity.Property(e => e.Biography).HasColumnType("text");

                entity.Property(e => e.IdParticipantType).HasColumnName("ID_ParticipantType");

                entity.Property(e => e.IdRepresentedCity).HasColumnName("ID_RepresentedCity");

                entity.Property(e => e.ParticipantDate)
                    .HasColumnName("Participant_Date")
                    .HasColumnType("date");

                entity.Property(e => e.ParticipantName)
                    .IsRequired()
                    .HasColumnName("Participant_Name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdParticipantTypeNavigation)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.IdParticipantType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Participants_ParticipantTypes");

                entity.HasOne(d => d.IdRepresentedCityNavigation)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.IdRepresentedCity)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Participants_Cities");
            });

            modelBuilder.Entity<Participations>(entity =>
            {
                entity.HasKey(e => e.IdParticipation);

                entity.Property(e => e.IdParticipation).HasColumnName("ID_Participation");

                entity.Property(e => e.IdNomination).HasColumnName("ID_Nomination");

                entity.Property(e => e.IdParticipant).HasColumnName("ID_Participant");

                entity.Property(e => e.IdSong).HasColumnName("ID_Song");

                entity.Property(e => e.IdYearOfContest).HasColumnName("ID_YearOfContest");

                entity.HasOne(d => d.IdNominationNavigation)
                    .WithMany(p => p.Participations)
                    .HasForeignKey(d => d.IdNomination)
                    .HasConstraintName("FK_Participations_Nominations");

                entity.HasOne(d => d.IdParticipantNavigation)
                    .WithMany(p => p.Participations)
                    .HasForeignKey(d => d.IdParticipant)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Participations_Participants");

                entity.HasOne(d => d.IdSongNavigation)
                    .WithMany(p => p.Participations)
                    .HasForeignKey(d => d.IdSong)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Participations_Songs");

                entity.HasOne(d => d.IdYearOfContestNavigation)
                    .WithMany(p => p.Participations)
                    .HasForeignKey(d => d.IdYearOfContest)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Participations_Years");
            });

            modelBuilder.Entity<Songs>(entity =>
            {
                entity.HasKey(e => e.IdSong);

                entity.Property(e => e.IdSong).HasColumnName("ID_Song");

                entity.Property(e => e.SongName)
                    .IsRequired()
                    .HasColumnName("Song_Name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Years>(entity =>
            {
                entity.HasKey(e => e.IdYearOfContest);

                entity.Property(e => e.IdYearOfContest).HasColumnName("ID_YearOfContest");

                entity.Property(e => e.IdHostCity).HasColumnName("ID_HostCity");

                entity.Property(e => e.Slogan)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Stage)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdHostCityNavigation)
                    .WithMany(p => p.Years)
                    .HasForeignKey(d => d.IdHostCity)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Years_Cities");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
