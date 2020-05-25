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
            var userClaims = await _userManager.GetClaimsAsync(userExists);
            result.Token = GenerateToken(userExists,userClaims);
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

            var userClaims = await GenerateClaimsUsers(newUser);
            result.Token = GenerateToken(newUser, userClaims);
            result.Success = resultnewUser.Succeeded;
            return result;
        }

        private string GenerateToken(IdentityUser user, IList<Claim> userClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);
            List<Claim> claims = new List<Claim>()
            {
                      new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                      new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),      
                      new Claim(JwtRegisteredClaimNames.Email,user.Email),
                      new Claim("id",user.Id)                        
            };
            
            if (userClaims!=null)
                claims.AddRange(userClaims);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            }; 
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
            
        }

        private async Task<IList<Claim>> GenerateClaimsUsers(IdentityUser user)
        {
            IList<Claim> result = null;
            if (user.Email.Contains("claim"))
            {
                 await _userManager.AddClaimAsync(user,new Claim("policiesclaim.view","true"));
                 result = await _userManager.GetClaimsAsync(user);
            }
            return result;
        }
    }
}