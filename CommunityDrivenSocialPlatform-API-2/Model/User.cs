using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CommunityDrivenSocialPlatform_API_2.Model
{
    [Table("user")]
    public partial class User
    {
        public User()
        {
            Comment = new HashSet<Comment>();
            Post = new HashSet<Post>();
            SubThread = new HashSet<SubThread>();
            SubThreadUser = new HashSet<SubThreadUser>();
            Vote = new HashSet<Vote>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("username")]
        [StringLength(100)]
        public string Username { get; set; }
        [Required]
        [Column("email_address")]
        [StringLength(255)]
        public string EmailAddress { get; set; }
        [Required]
        [Column("password")]
        [StringLength(255)]
        public string Password { get; set; }
        [Column("profile_picture_url", TypeName = "text")]
        public string ProfilePictureUrl { get; set; }
        [Column("description", TypeName = "text")]
        public string Description { get; set; }
        [Column("created_at", TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        [Column("role_id")]
        public int RoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        [InverseProperty("User")]
        public virtual Role Role { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Comment> Comment { get; set; }
        [InverseProperty("AuthorNavigation")]
        public virtual ICollection<Post> Post { get; set; }
        [InverseProperty("CreatorNavigation")]
        public virtual ICollection<SubThread> SubThread { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SubThreadUser> SubThreadUser { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Vote> Vote { get; set; }
    }
}
