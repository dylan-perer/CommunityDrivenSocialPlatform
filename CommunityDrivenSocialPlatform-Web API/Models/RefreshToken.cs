using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CDSP_API.Models
{
    [Table("refresh_token")]
    public partial class RefreshToken
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("token")]
        [StringLength(500)]
        public string Token { get; set; }
        [Required]
        [Column("jwt_id")]
        [StringLength(500)]
        public string JwtId { get; set; }
        [Column("created_at", TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        [Column("expire_at", TypeName = "datetime")]
        public DateTime ExpireAt { get; set; }
        [Column("is_used")]
        public bool IsUsed { get; set; }
        [Column("is_invalidated")]
        public bool IsInvalidated { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("RefreshToken")]
        public virtual User User { get; set; }
    }
}
