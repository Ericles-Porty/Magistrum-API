using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

public static class TokenService
{
    public static TokenModel GenerateToken(IdentityUser identityUser)
    {
        //take secret from appsettings.json
        var secret = new ConfigurationBuilder()
            .SetBasePath(Directory
            .GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build().GetSection("JWT")["Secret"];

        var tokenHandler = new JwtSecurityTokenHandler(); // tokenHandle faz a manipulação do token
        var key = Encoding.ASCII.GetBytes(secret!); // chave de criptografia
        var tokenDescriptor = new SecurityTokenDescriptor // descrição do token
        {
            Subject = new ClaimsIdentity(new Claim[]{ // identidade do token
                new Claim(ClaimTypes.NameIdentifier, identityUser.Id.ToString()), // claim é uma informação sobre o usuário
                new Claim(ClaimTypes.Name, identityUser.UserName!.ToString()), // claim é uma informação sobre o usuário
                new Claim(ClaimTypes.Role, "director")
            }),
            Expires = DateTime.UtcNow.AddHours(2), // tempo de expiração do token
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // criptografia do token
        };

        var token = new TokenModel
        {
            Token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor)),
            Expiration = tokenDescriptor.Expires.Value
        };

        return token; // retorna o token
    }
}