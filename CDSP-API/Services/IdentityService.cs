﻿using CDSP_API.Data;
using CDSP_API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using CDSP_API.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace CDSP_API.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly DataContext DataContext;
        private readonly IConfiguration Configuration;
        private readonly IUsersService UsersService;
        private readonly TokenValidationParameters TokenValidationParameters;
        public IdentityService(DataContext dataContext, IUsersService usersService, IConfiguration configuration, TokenValidationParameters tokenValidationParameters)
        {
            DataContext = dataContext;
            Configuration = configuration;
            UsersService = usersService;
            TokenValidationParameters = tokenValidationParameters;
        }
        public async Task<AuthResult> SigninAsync(User user)
        {
            User findUser = await DataContext.User.SingleOrDefaultAsync(r => r.Username == user.Username);

            if (findUser != null && VerifyPassword(user.Password, findUser.Password))
                return await GenerateToken(findUser);
            return null;
        }

        public async Task<AuthResult> SignupAsync(User user)
        {
            user.Password = HashPassword(user.Password);
            bool isSuccess = await UsersService.CreateAsync(user);

            if (isSuccess)
            {
                return await GenerateToken(user);
            }

            return null;
        }

        public async Task<AuthResult> GenerateToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));

            Role role = await DataContext.Role.SingleOrDefaultAsync(r => user.RoleId == r.Id);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Username),
                    new Claim(ClaimTypes.Role, role.RoleName),
                }),
                Expires = DateTime.UtcNow.AddSeconds(10),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            
            var jwtToken = jwtTokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken
            {
                JwtId = Guid.NewGuid().ToString(),
                IsUsed = false,
                IsInvalidated = false,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpireAt = DateTime.UtcNow.AddSeconds(15),
                Token = RandomString(35) + Guid.NewGuid()
            };

            await DataContext.RefreshToken.AddAsync(refreshToken);
            await DataContext.SaveChangesAsync();

            return new AuthResult
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                TokenCreatedAt = token.ValidFrom.ToLocalTime(),
                TokenExpireAt = token.ValidTo.ToLocalTime(),

                RefreshToken = refreshToken.Token,
                RefreshTokenCreatedAt = token.ValidFrom.ToLocalTime(),
                RefreshTokenExpireAt = token.ValidTo.ToLocalTime(),
            };
        }

        private string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJLKMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(x => x[random.Next(x.Length)]).ToArray());
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 4);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        public async Task<AuthResult> VerifyAndGenerateToken(string token, string refreshToken)
        {
            
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                //validate token string
                var tokenVerification = jwtTokenHandler.ValidateToken(token, TokenValidationParameters, out var validatedToken);
                
                //validate token encrypt algo
                if(validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (!result)
                    {
                        return new AuthResult { Errors = new[] {"Failed algoritm check"}};
                    }
                }

                //validate token expire
                var utcExpireDate = long.Parse(tokenVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expireDate = UnixTimeStampToDateTime(utcExpireDate);

                if(expireDate < DateTime.UtcNow)
                {
                    return new AuthResult
                    {
                        isSuccess = false,
                        Errors = new[] { "Token has not expired yet." }
                    };
                }

                //validate if token exists in datasource
                var storedToken = await DataContext.RefreshToken.FirstOrDefaultAsync(r=>r.Token.Equals(refreshToken));
                if(storedToken is null)
                {
                    return new AuthResult
                    {
                        isSuccess = false,
                        Errors = new[] { "Token does not exist." }
                    };
                }

                //see if token is used
                if (storedToken.IsUsed)
                {
                    return new AuthResult
                    {
                        isSuccess = false,
                        Errors = new[] { "Token is used." }
                    };
                }

                //see if token is invalidated
                if (storedToken.IsInvalidated)
                {
                    return new AuthResult
                    {
                        isSuccess = false,
                        Errors = new[] { "Token is invalidated." }
                    };
                }

                //update current token
                storedToken.IsUsed = true;
                DataContext.RefreshToken.Update(storedToken);
                await DataContext.SaveChangesAsync();

                //create new token
                User loggedUser = await DataContext.User.SingleOrDefaultAsync(r=> r.Id == storedToken.UserId);
                if (loggedUser is null)
                {
                    return new AuthResult
                    {
                        Errors = new[] {"Is nullwtf"}
                    };
                }
                return await GenerateToken(loggedUser);
            }
            catch(Exception ex)
            {
                return new AuthResult
                {
                    Errors = new[] {ex.Message}
                };
            }
        }

        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
}
