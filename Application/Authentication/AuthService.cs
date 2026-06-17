using Application.Authentication.DTOs;

namespace Application.Authentication;

public class AuthService : IAuthService
{
    public Task<SignInResponse> SignInAsync(SignInRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<SignUpResponse> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<SignInResponse> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SignOutAsync(string refreshToken, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}