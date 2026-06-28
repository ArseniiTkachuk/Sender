using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _key;

    public TokenService(IConfiguration config)
    {
        var tokenKey = config["TokenKey"];
        if (string.IsNullOrEmpty(tokenKey))
            throw new ArgumentNullException("TokenKey", "Ключ 'TokenKey' не знайдено в appsettings.json");

        if (tokenKey.Length < 32)
            throw new InvalidOperationException("Ключ 'TokenKey' має бути мінімум 32 символи довжиною.");

        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
    }

    public string CreateToken<T>(T obj, int existDays)
    {
        var claims = new List<Claim>();

        foreach (var prop in typeof(T).GetProperties())
        {
            var value = prop.GetValue(obj)?.ToString();
            if (value != null)
                claims.Add(new Claim(prop.Name, value));
        }

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(existDays),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
    }

    public T DecodeToken<T>(string token) where T : new()
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _key,
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        }, out SecurityToken validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;
        var obj = new T();

        foreach (var prop in typeof(T).GetProperties())
        {
            var claim = jwtToken.Claims.FirstOrDefault(c => c.Type == prop.Name);
            if (claim != null)
            {
                object value;
                // Перевіряємо, чи є тип властивості Guid
                if (prop.PropertyType == typeof(Guid))
                {
                    value = Guid.Parse(claim.Value);
                }
                else
                {
                    // Для інших типів залишаємо стандартний метод
                    value = Convert.ChangeType(claim.Value, prop.PropertyType);
                }

                prop.SetValue(obj, value);
            }
        }
        return obj;
    }
}