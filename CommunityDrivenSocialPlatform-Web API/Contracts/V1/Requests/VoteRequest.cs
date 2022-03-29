using CDSP_API.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace CDSP_API.Contracts.V1.Requests
{
    public class VoteRequest
    {
        [Required]
        public PostVoteEnum voteType { get; set; }
    }
}
