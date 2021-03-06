﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Security.Principal;
using PoohAPI.Models;
using Microsoft.Extensions.Configuration;

namespace PoohAPI.Authorization
{
    public class TokenHelper : ITokenHelper
    {
        private readonly IConfiguration _configSettings;
        public TokenHelper(IConfiguration configSettings)
        {
            _configSettings = configSettings;
        }

        private string RequestToken(ClaimsIdentity user, int expiryTimeInSeconds)
        {
            var claims = new List<Claim>()
            {
                user.FindFirst("id"),
                user.FindFirst("active"),
                user.FindFirst(JwtRegisteredClaimNames.Iat),
                user.FindFirst(ClaimTypes.Role),
                user.FindFirst("refreshToken")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configSettings.GetValue<string>("JWTSigningKey")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configSettings.GetValue<string>("JWTIssuer"),
                audience: _configSettings.GetValue<string>("JWTAudience"),
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(expiryTimeInSeconds),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsIdentity CreateClaimsIdentity(bool activeUser, int userId, string userRole, string refreshToken = null)
        {
            return new ClaimsIdentity(new GenericIdentity(userId.ToString(), "Token"), new[]
            {
                new Claim("active", activeUser.ToString()),
                new Claim("id", userId.ToString(), ClaimValueTypes.Integer32),
                new Claim("refreshToken", string.IsNullOrWhiteSpace(refreshToken) ? "" : refreshToken),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.Role, userRole)
            });
        }

        /// <summary>
        /// Generates a JWT token. Giving the expiryTimeInSeconds a value of '0' will make it use the value defined in the applicationsettings.
        /// </summary>
        public string GenerateJWT(ClaimsIdentity user, int expiryTimeInSeconds = 0)
        {
            if (expiryTimeInSeconds == 0)
                expiryTimeInSeconds = _configSettings.GetValue<int>("JWTExpiryTime");
            return RequestToken(user, expiryTimeInSeconds);
        }
    }
}

