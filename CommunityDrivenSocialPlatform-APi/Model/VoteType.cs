﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CommunityDrivenSocialPlatform_APi.Model
{
    [Table("vote_type")]
    public partial class VoteType
    {
        [Key]
        [Column("id")]
        public byte Id { get; set; }
        [Required]
        [Column("vote_type_name")]
        [StringLength(10)]
        public string VoteTypeName { get; set; }
    }
}
