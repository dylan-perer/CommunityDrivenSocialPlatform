using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using CDSP_API.Models;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CDSP_API.Data
{
    public partial class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<ErrorLog> ErrorLog { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<RefreshToken> RefreshToken { get; set; }
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
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FKcomment637670");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKcomment869114");
            });

            modelBuilder.Entity<ErrorLog>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Body).IsUnicode(false);

                entity.Property(e => e.HttpAction).IsUnicode(false);

                entity.Property(e => e.RouteName).IsUnicode(false);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.Title).IsUnicode(false);

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKpost651551");

                entity.HasOne(d => d.SubThread)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.SubThreadId)
                    .HasConstraintName("FKpost347750");
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.Property(e => e.JwtId).IsUnicode(false);

                entity.Property(e => e.Token).IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RefreshToken)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FKrefresh_to567261");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleName).IsUnicode(false);
            });

            modelBuilder.Entity<SubThread>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ__sub_thre__72E12F1B3A41791B")
                    .IsUnique();

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.SubThread)
                    .HasForeignKey(d => d.CreatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKsub_thread692899");
            });

            modelBuilder.Entity<SubThreadRole>(entity =>
            {
                entity.HasIndex(e => e.SubThreadRoleName)
                    .HasName("UQ__sub_thre__AC26C1506751022F")
                    .IsUnique();

                entity.Property(e => e.SubThreadRoleName)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(user_name())");
            });

            modelBuilder.Entity<SubThreadUser>(entity =>
            {
                entity.HasKey(e => new { e.SubThreadId, e.UserId })
                    .HasName("PK__sub_thre__24F7E31E88E5D0D3");

                entity.HasOne(d => d.SubThread)
                    .WithMany(p => p.SubThreadUser)
                    .HasForeignKey(d => d.SubThreadId)
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
                    .HasName("UQ__user__20C6DFF56FBC9321")
                    .IsUnique();

                entity.HasIndex(e => e.Username)
                    .HasName("UQ__user__F3DBC5722F2902D7")
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
                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Vote)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FKvote136129");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Vote)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKvote904684");

                entity.HasOne(d => d.VoteType)
                    .WithMany(p => p.Vote)
                    .HasForeignKey(d => d.VoteTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKvote232645");
            });

            modelBuilder.Entity<VoteType>(entity =>
            {
                entity.HasIndex(e => e.VoteTypeName)
                    .HasName("UQ__vote_typ__75D83FD842A52A33")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.VoteTypeName).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    
}
}
