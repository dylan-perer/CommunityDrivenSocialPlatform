using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CommunityDrivenSocialPlatform_APi.Model
{
    [Table("vote")]
    public partial class Vote
    {
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("post_id")]
        public int PostId { get; set; }
        [Column("vote_type_id")]
        public byte VoteTypeId { get; set; }

        [ForeignKey(nameof(PostId))]
        public virtual Post Post { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
        [ForeignKey(nameof(VoteTypeId))]
        public virtual VoteType VoteType { get; set; }
    }
}
