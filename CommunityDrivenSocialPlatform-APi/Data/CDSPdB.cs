using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using CommunityDrivenSocialPlatform_APi.Model;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CommunityDrivenSocialPlatform_APi.Data
{
    public partial class CDSPdB : DbContext
    {
        public CDSPdB()
        {
        }

        public CDSPdB(DbContextOptions<CDSPdB> options)
            : base(options)
        {
        }

        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<SubThread> SubThread { get; set; }
        public virtual DbSet<SubThreadRole> SubThreadRole { get; set; }
        public virtual DbSet<SubThreadUser> SubThreadUser { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Vote> Vote { get; set; }
        public virtual DbSet<VoteType> VoteType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=CommunityDrivenSocialPlatform;User ID=dylan;Password=sa");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.Title).IsUnicode(false);

                entity.HasOne(d => d.AuthorNavigation)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.Author)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKPost233543");

                entity.HasOne(d => d.SubThread)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.SubThreadId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKPost301063");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleName).IsUnicode(false);
            });

            modelBuilder.Entity<SubThread>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ__sub_thre__72E12F1B4E7C7F3B")
                    .IsUnique();

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.CreatorNavigation)
                    .WithMany(p => p.SubThread)
                    .HasForeignKey(d => d.Creator)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKsub_thread85277");
            });

            modelBuilder.Entity<SubThreadRole>(entity =>
            {
                entity.HasIndex(e => e.SubThreadRoleName)
                    .HasName("UQ__sub_thre__AC26C15036EBD113")
                    .IsUnique();

                entity.Property(e => e.SubThreadRoleName)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(user_name())");
            });

            modelBuilder.Entity<SubThreadUser>(entity =>
            {
                entity.HasKey(e => new { e.SubThreadId, e.UserId })
                    .HasName("PK__sub_thre__24F7E31E5F8CC561");

                entity.HasOne(d => d.SubThread)
                    .WithMany(p => p.SubThreadUser)
                    .HasForeignKey(d => d.SubThreadId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKsub_thread508414");

                entity.HasOne(d => d.SubThreadRole)
                    .WithMany(p => p.SubThreadUser)
                    .HasForeignKey(d => d.SubThreadRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKsub_thread562234");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SubThreadUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKsub_thread755888");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.EmailAddress)
                    .HasName("UQ__user__20C6DFF54FAC8395")
                    .IsUnique();

                entity.HasIndex(e => e.Username)
                    .HasName("UQ__user__F3DBC5726283B325")
                    .IsUnique();

                entity.Property(e => e.EmailAddress).IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.Username).IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKuser994439");
            });

            modelBuilder.Entity<Vote>(entity =>
            {
                entity.HasNoKey();

                entity.HasOne(d => d.Post)
                    .WithMany()
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKvote89442");

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKvote904684");

                entity.HasOne(d => d.VoteType)
                    .WithMany()
                    .HasForeignKey(d => d.VoteTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKvote232645");
            });

            modelBuilder.Entity<VoteType>(entity =>
            {
                entity.HasIndex(e => e.VoteTypeName)
                    .HasName("UQ__vote_typ__75D83FD828393082")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.VoteTypeName).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
