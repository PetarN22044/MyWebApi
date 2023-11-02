using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

namespace MyWebApi.Data
{///...
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
       private readonly IConfiguration _configuration;
        public AuthRepository(DataContext context,IConfiguration configuration){
           _configuration = configuration;
            _context = context; 
        }
        public async Task<ServiceResponce<string>> Login(string username, string password)
        {
           var response = new ServiceResponce<string>();
           var user = await _context.Users.FirstOrDefaultAsync(x=>x.UserName.ToLower().Equals(username.ToLower()));
            if(user is null){
                response.Success = false;
                response.Message = "User not found";

            }
            else if (!VerifyPasswordHash(password,user.PasswordHash,user.PasswordSalt)){
                response.Success = true;
                response.Message = "Wrong password";
            }else{
                response.Data = CreateToken(user);
            }
            return response;
               

        }

        public async Task<ServiceResponce<int>> Register(User user, string password)
        {
            var response = new ServiceResponce<int>();
            if(await UserExist(user.UserName)){
                response.Success = false;
                response.Message = "User alredy exist";
                return response;
            }


            CreatePasswordHas(password,out byte[] passwordHas,out byte[] passwordSalt);
             user.PasswordHash = passwordHas;
             user.PasswordSalt = passwordSalt;



            _context.Users.Add(user);
            await _context.SaveChangesAsync();

             response.Data = user.Id;
            return response;
        }

        public async Task<bool> UserExist(string username)
        {
            if(await _context.Users.AnyAsync(x=>x.UserName.ToLower()== username.ToLower())){
                return true;
            }
            return false;
        }


        private void CreatePasswordHas (string password,out byte[] passwordHash,out byte[] passwordSalt){
           
           using(var hmac = new System.Security.Cryptography.HMACSHA512()){
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

           }


        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash,byte[]passwordSalt){
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)){
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken (User user){

         var claims = new List<Claim> {
            new Claim (ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim(ClaimTypes.Name,user.UserName)
         }    ; 

         var appSettingsToken = _configuration.GetSection("AppSettings:Token").Value;      
          if(appSettingsToken is null)
          throw new Exception("AppSettings Token is Empty");
          SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(appSettingsToken));
          SigningCredentials creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

          var tokenDescriptor = new SecurityTokenDescriptor{
Subject = new ClaimsIdentity(claims),
Expires = DateTime.Now.AddDays(1),
SigningCredentials = creds
          };

          JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
          SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        return  tokenHandler.WriteToken(token);

           
        }
    }
}