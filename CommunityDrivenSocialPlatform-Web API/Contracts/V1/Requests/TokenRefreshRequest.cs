using System.ComponentModel.DataAnnotations;

namespace CDSP_API.Data
{
    public class TokenRefreshRequest
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}
