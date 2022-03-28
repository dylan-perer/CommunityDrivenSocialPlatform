using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CDSP_API.Models
{
    [Table("vote")]
    public partial class Vote
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("post_id")]
        public int PostId { get; set; }
        [Column("vote_type_id")]
        public byte VoteTypeId { get; set; }

        [ForeignKey(nameof(PostId))]
        [InverseProperty("Vote")]
        public virtual Post Post { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("Vote")]
        public virtual User User { get; set; }
        [ForeignKey(nameof(VoteTypeId))]
        [InverseProperty("Vote")]
        public virtual VoteType VoteType { get; set; }
    }
}
