using Financas.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SegurancaJWT.Services
{
    public class JwtTokenService
    {
        public string Create(Usuario user)
        {
            var handler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes("chaveespecialparavalidacao"); /* Implementar para buscar atraves do appsettings */
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescription = new SecurityTokenDescriptor
            {
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddHours(4),
                Subject = GenerateClaims(user)
            };

            var token = handler.CreateToken(tokenDescription);
            return handler.WriteToken(token);
        }
        private static ClaimsIdentity GenerateClaims(Usuario user)
        {
            var ci = new ClaimsIdentity();

            ci.AddClaim(new Claim("UsuarioId", user.UsuarioId.ToString()));
            ci.AddClaim(new Claim(ClaimTypes.Name, user.Email));
            ci.AddClaim(new Claim(ClaimTypes.Email, user.Email));

            return ci;
        }
    }
}
