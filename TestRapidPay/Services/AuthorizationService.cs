using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestRapidPay.Models;

namespace TestRapidPay.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly RapidPayContext _context;
        private readonly IConfiguration _configuration;

        public AuthorizationService(RapidPayContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private string GenerateToken(string UserId)
        {
            var key = _configuration.GetValue<string>("JwtSettings:key");
            var keyBytes = Encoding.ASCII.GetBytes(key);

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, UserId));

            var credentialsToken = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256Signature
                );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = credentialsToken
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            var tokenCreated = tokenHandler.WriteToken(tokenConfig);

            return tokenCreated;
        }



        public async Task<AuthorizationResponse> ReturnToken(User authorization)
        {
            var user = _context.Users.FirstOrDefault(x =>
            x.Email == authorization.Email &&
            x.PasswordHash == authorization.PasswordHash);

            if (user == null)
            {
                return await Task.FromResult<AuthorizationResponse>(null);
            }

            string tokenCreated = GenerateToken(user.UserId.ToString());

            return new AuthorizationResponse() { Token = tokenCreated, Result = true, Msg = "Ok" };
        }
    }
}
