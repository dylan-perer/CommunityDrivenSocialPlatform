using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CommunityDrivenSocialPlatform_APi.Model
{
    [Table("sub_thread_user")]
    public partial class SubThreadUser
    {
        [Key]
        [Column("sub_thread_id")]
        public int SubThreadId { get; set; }
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("sub_thread_role_id")]
        public int SubThreadRoleId { get; set; }

        [ForeignKey(nameof(SubThreadId))]
        [InverseProperty("SubThreadUser")]
        public virtual SubThread SubThread { get; set; }
        [ForeignKey(nameof(SubThreadRoleId))]
        [InverseProperty("SubThreadUser")]
        public virtual SubThreadRole SubThreadRole { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("SubThreadUser")]
        public virtual User User { get; set; }
    }
}
