using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CDSP_API.Models
{
    [Table("error_log")]
    public partial class ErrorLog
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("logged_user_id")]
        public int? LoggedUserId { get; set; }
        [Required]
        [Column("route_name")]
        [StringLength(255)]
        public string RouteName { get; set; }
        [Required]
        [Column("http_action")]
        [StringLength(10)]
        public string HttpAction { get; set; }
        [Column("body")]
        [StringLength(1000)]
        public string Body { get; set; }
        [Required]
        [Column("error", TypeName = "text")]
        public string Error { get; set; }
        [Column("occured_at", TypeName = "datetime")]
        public DateTime OccuredAt { get; set; }
    }
}
