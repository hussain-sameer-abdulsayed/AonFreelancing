﻿using AonFreelancing.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using AonFreelancing.JwtClasses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AonFreelancing.Repositories.IRepos;
using AonFreelancing.Models;

namespace AonFreelancing.Repositories.Repos
{
    public class JWTManagerRepo : IJWTMangerRepo
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly MyContext _context;

        public JWTManagerRepo(IConfiguration configuration, UserManager<User> userManager, MyContext context, RoleManager<IdentityRole> roleManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }
        public async Task<MyToken> GenerateToken(string userName)
        {
            return await GenerateJWTTokens(userName);
        }
        public async Task<MyToken> GenerateRefreshToken(string userName)
        {
            return await GenerateJWTTokens(userName);
        }
        // this generate token and refresh token depending which method was calling the two above calls this method
        public async Task<MyToken> GenerateJWTTokens(string userName)
        {
            try
            {
                int ExpiresDuration = Convert.ToInt32(_configuration["Jwt:ExpireInMinutes"]);
                var user = await _userManager.FindByNameAsync(userName);
                var userClaims = await _userManager.GetClaimsAsync(user);
                var roles = await _userManager.GetRolesAsync(user);

                /*
                var userType = userRoles.Any(r => r == "systemadmin");
                var isClient = userRoles.Any(r => r == "client");
                var isFreelancer = userRoles.Any(r => r == "freelancer");
                */

                const int expireTimeInMinutes = 3600;
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userName),
                    new Claim("uid", user.Id),
                    new Claim("phonenumber", user.PhoneNumber),
                    new Claim(ClaimTypes.Role,roles.First()),
                    new Claim(JwtRegisteredClaimNames.Aud, _configuration["Jwt:Audience"]),
                    new Claim(JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"])
                };


                // Add role claims dynamically
                // Add roles as claims
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                claims.AddRange(userClaims); // Merge user-specific claims

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddMinutes(expireTimeInMinutes),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var refreshToken = GenerateRefreshToken();

                return new MyToken { AccessToken = tokenHandler.WriteToken(token), RefreshToken = refreshToken };
            }
            catch
            {
                return null;
            }
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var Key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
        public string GetUserId(string mytoken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(mytoken);

            var userIdClaim = token.Claims.FirstOrDefault(claim => claim.Type == "nameid");

            if (userIdClaim != null)
            {
                string userId = userIdClaim.Value;
                // Use userId as needed
                return userId;
            }
            else
            {
                return null;
            }
        }


        //private methods
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        private async Task<bool> IsValidToken(string token)
        {
            try
            {
                var chk = await _context.UserRefreshTokens.Where(t => t.RefreshToken == token).FirstOrDefaultAsync();
                if (chk is null)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        private JwtSecurityToken ConvertJwtStringToJwtSecurityToken(string? jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            return token;
        }
        private class DecodedToken
        {
            public string KeyId { get; set; }
            public string Issuer { get; set; }
            public List<string> Audiences { get; set; }
            public List<(string Type, string Value)> Claims { get; set; }
            public DateTime ValidTo { get; set; }
            public string SignatureAlgorithm { get; set; }
            public byte[] RawData { get; set; }
            public string Subject { get; set; }
            public DateTime ValidFrom { get; set; }
            public string EncodedHeader { get; set; }
            public string EncodedPayload { get; set; }

            public DecodedToken(
                string keyId,
                string issuer,
                List<string> audiences,
                List<(string Type, string Value)> claims,
                DateTime validTo,
                string signatureAlgorithm,
                string subject,
                DateTime validFrom,
                string encodedHeader,
                string encodedPayload)
            {
                KeyId = keyId;
                Issuer = issuer;
                Audiences = audiences;
                Claims = claims;
                ValidTo = validTo;
                SignatureAlgorithm = signatureAlgorithm;
                Subject = subject;
                ValidFrom = validFrom;
                EncodedHeader = encodedHeader;
                EncodedPayload = encodedPayload;
            }
        }
        private string GetUserIdFromClaims(List<(string Type, string Value)> claims)
        {
            // Here you should search for the claim that represents the user ID.
            // Replace "UserIdClaimType" with the actual type of claim representing the user ID.
            var userIdClaim = claims.FirstOrDefault(claim => claim.Type == "nameid");

            if (userIdClaim != default)
            {
                return userIdClaim.Value;
            }

            // If the user ID claim is not found, you can return null or an empty string, or handle it differently based on your requirement.
            return null;
        }
    }
}
