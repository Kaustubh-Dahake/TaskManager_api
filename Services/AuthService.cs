using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManager_api.Models;

namespace TaskManager_api.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;
        private readonly List<User> _users;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;

            _users = new List<User>
            {
              new User { Username = "admin1", Password = "admin123", Role = "Admin", Logo = "https://ui-avatars.com/api/?name=Admin+1&background=0D8ABC&color=fff" },
new User { Username = "user1", Password = "user@123", Role = "User", Logo = "https://ui-avatars.com/api/?name=User+1&background=1abc9c&color=fff" },
new User { Username = "admin2", Password = "admin456", Role = "Admin", Logo = "https://ui-avatars.com/api/?name=Admin+2&background=0D8ABC&color=fff" },
new User { Username = "user2", Password = "user456", Role = "User", Logo = "https://ui-avatars.com/api/?name=User+2&background=1abc9c&color=fff" },
new User { Username = "admin3", Password = "admin789", Role = "Admin", Logo = "https://ui-avatars.com/api/?name=Admin+3&background=0D8ABC&color=fff" },
new User { Username = "user3", Password = "user789", Role = "User", Logo = "https://ui-avatars.com/api/?name=User+3&background=1abc9c&color=fff" },
new User { Username = "admin4", Password = "admin101", Role = "Admin", Logo = "https://ui-avatars.com/api/?name=Admin+4&background=0D8ABC&color=fff" },
new User { Username = "user4", Password = "user101", Role = "User", Logo = "https://ui-avatars.com/api/?name=User+4&background=1abc9c&color=fff" },

            };
        }

        public AuthResult? Authenticate(LoginDto loginDto)
        {
            var user = _users.SingleOrDefault(u => u.Username == loginDto.Username && u.Password == loginDto.Password);
            if (user == null)
                return null;

            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return new AuthResult
            {
                Username = user.Username,
                Role = user.Role,
                Logo = user.Logo,
                Token = jwt
            };
        }
    }
}
