using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TweetBook.Domain;
using TweetBook.Options;

namespace TweetBook.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser>_userManager;
        private readonly JwtSettingsOptions _jwtOptions;

        public IdentityService(UserManager<IdentityUser> userManager, JwtSettingsOptions jwtOptions)
        {
            _userManager = userManager;
            _jwtOptions = jwtOptions;
        }

        public  async Task<AuthenticationResult>  LoginAsync(string email, string password)
        {
            var result = new AuthenticationResult();

             var userExists =  await _userManager.FindByEmailAsync(email);
            
            if (userExists== null)
            {
               result.Errors = new List<string>() { "Usuário não existe" };     
               return result;
            }

            var resultLogin = await _userManager.CheckPasswordAsync(userExists, password);

            if (!resultLogin)
            {
               result.Errors = new List<string>() { "Password incorreto" };     
               return result;
            }
            result.Token = GenerateToken(userExists);
            result.Success = resultLogin;
            return result;


        }

        public async Task<AuthenticationResult> RegisterAsync(string email, string password)
        {
            var result = new AuthenticationResult();
            
            var userExists =  await _userManager.FindByEmailAsync(email);
            
            if (userExists!= null)
            {
               result.Errors = new List<string>() { "Usuário já existe" };     
               return result;
            }
            
            var newUser =  new IdentityUser()
            {
                UserName =email, 
                Email = email
            };

            var resultnewUser = await _userManager.CreateAsync(newUser, password);

            if (!resultnewUser.Succeeded)
            {
                result.Errors = resultnewUser.Errors.Select(x=> x.Description).ToList();
                return result;
            }
            result.Token = GenerateToken(newUser);
            result.Success = resultnewUser.Succeeded;
            return result;
        }

        private string GenerateToken(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity( new[]
                    {
                      new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                      new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),      
                      new Claim(JwtRegisteredClaimNames.Email,user.Email),
                      new Claim("id",user.Id)                        
                    }
                ),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            }; 
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
            
        }
    }
}