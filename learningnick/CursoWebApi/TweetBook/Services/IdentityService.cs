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

        private readonly RoleManager<IdentityRole>_roleManager;

        public IdentityService(UserManager<IdentityUser> userManager, JwtSettingsOptions jwtOptions, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _jwtOptions = jwtOptions;
            _roleManager = roleManager;
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
            var userRoles = await _userManager.GetRolesAsync(userExists);
            result.Token = GenerateToken(userExists,userClaims,userRoles);
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
            result.Token = GenerateToken(newUser, userClaims,null);
            result.Success = resultnewUser.Succeeded;
            return result;
        }

        private string GenerateToken(IdentityUser user, IList<Claim> userClaims, IList<string> userRoles)
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

            if (userRoles!=null)                
                claims.Add(new Claim(ClaimTypes.Role,userRoles.First()));


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

        public async Task<IEnumerable<string>> CreateRoles()
        {
            List<string> rolesName = null;
            
            if (!(await _roleManager.RoleExistsAsync("Admin")))
                await _roleManager.CreateAsync(new IdentityRole(){Name="Admin"});
            
            if (!(await _roleManager.RoleExistsAsync("User")))
                await _roleManager.CreateAsync(new IdentityRole(){Name="User"});
            
            rolesName = _roleManager.Roles.Select(x=> x.Name).ToList();
            
            return rolesName;
            
        }
    }
}