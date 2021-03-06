using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CDSP_API.Models
{
    [Table("comment")]
    public partial class Comment
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("post_id")]
        public int PostId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Required]
        [Column("body", TypeName = "text")]
        public string Body { get; set; }
        [Column("created_at", TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(PostId))]
        [InverseProperty("Comment")]
        public virtual Post Post { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("Comment")]
        public virtual User User { get; set; }
    }
}
