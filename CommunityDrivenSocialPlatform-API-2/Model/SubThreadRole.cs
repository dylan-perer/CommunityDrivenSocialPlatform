using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CommunityDrivenSocialPlatform_API_2.Model
{
    [Table("sub_thread_role")]
    public partial class SubThreadRole
    {
        public SubThreadRole()
        {
            SubThreadUser = new HashSet<SubThreadUser>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("sub_thread_role_name")]
        [StringLength(100)]
        public string SubThreadRoleName { get; set; }

        [InverseProperty("SubThreadRole")]
        public virtual ICollection<SubThreadUser> SubThreadUser { get; set; }
    }
}
