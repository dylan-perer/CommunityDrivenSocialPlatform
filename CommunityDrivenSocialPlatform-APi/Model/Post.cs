using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CommunityDrivenSocialPlatform_APi.Model
{
    [Table("post")]
    public partial class Post
    {
        public Post()
        {
            Comment = new HashSet<Comment>();
            Vote = new HashSet<Vote>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("title")]
        [StringLength(255)]
        public string Title { get; set; }
        [Required]
        [Column("body", TypeName = "text")]
        public string Body { get; set; }
        [Column("created_at", TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        [Column("sub_thread_id")]
        public int SubThreadId { get; set; }
        [Column("author")]
        public int Author { get; set; }

        [ForeignKey(nameof(Author))]
        [InverseProperty(nameof(User.Post))]
        public virtual User AuthorNavigation { get; set; }
        [ForeignKey(nameof(SubThreadId))]
        [InverseProperty("Post")]
        public virtual SubThread SubThread { get; set; }
        [InverseProperty("Post")]
        public virtual ICollection<Comment> Comment { get; set; }
        [InverseProperty("Post")]
        public virtual ICollection<Vote> Vote { get; set; }
    }
}
