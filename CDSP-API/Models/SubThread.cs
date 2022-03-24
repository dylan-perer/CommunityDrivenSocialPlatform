using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CDSP_API.Models
{
    [Table("sub_thread")]
    public partial class SubThread
    {
        public SubThread()
        {
            Post = new HashSet<Post>();
            SubThreadUser = new HashSet<SubThreadUser>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("name")]
        [StringLength(150)]
        public string Name { get; set; }
        [Required]
        [Column("description", TypeName = "text")]
        public string Description { get; set; }
        [Required]
        [Column("welcome_message", TypeName = "text")]
        public string WelcomeMessage { get; set; }
        [Column("creator_id")]
        public int CreatorId { get; set; }
        [Column("created_at", TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(CreatorId))]
        [InverseProperty(nameof(User.SubThread))]
        public virtual User Creator { get; set; }
        [InverseProperty("SubThread")]
        public virtual ICollection<Post> Post { get; set; }
        [InverseProperty("SubThread")]
        public virtual ICollection<SubThreadUser> SubThreadUser { get; set; }
    }
}
