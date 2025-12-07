namespace BaskIt.Services.Jwt;

public interface IJwtService
{
    string GenerateJwtToken(string userId, string email);
}
