namespace RealEstateApi.Services.AuthService
{
    public interface IAuthService
    {
        Task<Token> GenerateToken(string email);
    }
}
