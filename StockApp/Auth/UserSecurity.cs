using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Cryptography;

namespace StockApp.Auth
{
    public class UserSecurity
    {
        public static string Secret = "ThisisaSecret!";
        public static bool Login(string UserName, string Password)
        {
            if(UserName == "odmen")
                return true;
            return false;
            //return UserName == "odmen" ? true : false;
     }
       /* public static string GenerateToken(string username)
        {
            var symmetricKey = Convert.FromBase64String(Secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Name, user.Username),
                            new Claim(ClaimTypes.Role, ((Roles)user.RoleId).ToString()),
                            new Claim("guid",user.Guid)
                        }),

                Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }*/
       /* public static string GenerateToken(string username)
        {
            byte[] key = Convert.FromBase64String(Secret);
            var securityKey = new System.IdentityModel.Tokens.SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new System.IdentityModel.Tokens.Jwt.seSecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {  
                new Claim(ClaimTypes.Name, username)  
            }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }*/

        /*public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null) return null;
                byte[] key = Convert.FromBase64String(Secret);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }  */
    }
}