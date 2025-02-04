using API.DTOs;

namespace API.Interfaces
{
    public interface IAuthService
    {
        Task<AuthDto> RegisterAsync(RegisterDto dto);
        Task<AuthDto> GetTokenAsync(LoginDto dto);
        Task<AuthDto> RefreshTokenAsync(string refreshToken);
        //Task<bool> RevokRefreshToken(string refreshToken);
    }
}
