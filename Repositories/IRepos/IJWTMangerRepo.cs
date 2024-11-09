﻿using AonFreelancing.JwtClasses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AonFreelancing.Repositories.IRepos
{
    public interface IJWTMangerRepo
    {
        Task<MyToken> GenerateToken(string userName);
        Task<MyToken> GenerateRefreshToken(string userName);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        //Task<bool> IsValidToken(string token);
        //JwtSecurityToken ConvertJwtStringToJwtSecurityToken(string? jwt);
        string GetUserId(string mytoken);
    }
}