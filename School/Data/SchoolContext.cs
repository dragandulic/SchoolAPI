using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace School.Data
{
    public partial class SchoolContext : DbContext
    {
        public SchoolContext()
        {
        }

        public SchoolContext(DbContextOptions<SchoolContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Academy> Academy { get; set; }
        public virtual DbSet<Announcement> Announcement { get; set; }
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Class> Class { get; set; }
        public virtual DbSet<ClassPerson> ClassPerson { get; set; }
        public virtual DbSet<ClassSubject> ClassSubject { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Subject> Subject { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=School;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Academy>(entity =>
            {
                entity.Property(e => e.Location).HasMaxLength(250);

                entity.Property(e => e.Name).HasMaxLength(250);
            });

            modelBuilder.Entity<Announcement>(entity =>
            {
                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Announcement)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Announcement_Class");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Announcement)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("FK_Announcement_Person");
            });

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("FK_AspNetUsers_Person");
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.Property(e => e.Generation).HasMaxLength(250);

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.HasOne(d => d.Academy)
                    .WithMany(p => p.Class)
                    .HasForeignKey(d => d.AcademyId)
                    .HasConstraintName("FK_Class_Academy");
            });

            modelBuilder.Entity<ClassPerson>(entity =>
            {
                entity.HasKey(e => new { e.ClassId, e.PersonId });

                entity.ToTable("Class_Person");

                entity.Property(e => e.Mark).HasMaxLength(50);

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.ClassPerson)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK_Class_Person_Class");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.ClassPerson)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("FK_Class_Person_Person");
            });

            modelBuilder.Entity<ClassSubject>(entity =>
            {
                entity.HasKey(e => new { e.ClassId, e.SubjectId });

                entity.ToTable("Class_Subject");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.ClassSubject)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Class_Subject_Class");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.ClassSubject)
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("FK_Class_Subject_Subject");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.HasOne(d => d.Announcement)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.AnnouncementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_Announcement");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("FK_Comment_Person");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.FirstName).HasMaxLength(250);

                entity.Property(e => e.LastName).HasMaxLength(250);

                entity.HasOne(d => d.MyParent)
                    .WithMany(p => p.InverseMyParent)
                    .HasForeignKey(d => d.MyParentId)
                    .HasConstraintName("FK_Person_Person");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(250);

                entity.HasOne(d => d.Academy)
                    .WithMany(p => p.Subject)
                    .HasForeignKey(d => d.AcademyId)
                    .HasConstraintName("FK_Subject_Academy");

                entity.HasOne(d => d.Professor)
                    .WithMany(p => p.Subject)
                    .HasForeignKey(d => d.ProfessorId)
                    .HasConstraintName("FK_Subject_Person");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
