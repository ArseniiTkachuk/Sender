public interface ITokenService
{
    string CreateToken<T>(T obj, int existDays = 7);
    T DecodeToken<T>(string token) where T : new();
}