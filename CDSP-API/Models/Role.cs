using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CDSP_API.Models
{
    [Table("role")]
    public partial class Role
    {
        public Role()
        {
            User = new HashSet<User>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("role_name")]
        [StringLength(100)]
        public string RoleName { get; set; }

        [InverseProperty("Role")]
        public virtual ICollection<User> User { get; set; }
    }
}
