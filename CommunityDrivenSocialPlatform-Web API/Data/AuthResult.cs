using System;

namespace CDSP_API.Data
{
    public class AuthResult
    {
        public string Token { get; set; }
        public DateTime TokenCreatedAt { get; set; } 
        public DateTime TokenExpireAt { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenCreatedAt{ get; set; } 
        public DateTime RefreshTokenExpireAt { get; set; }
        public string[] Errors { get; set; }

        public bool isSuccess { get; set; }

        
        
    }
}
