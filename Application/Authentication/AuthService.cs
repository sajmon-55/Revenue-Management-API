using Application.Authentication.DTOs;
using Application.Exceptions;
using Domain.Entities;
using Infrastructure.Authentication;
using Infrastructure.Authentication.Password;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Authentication;

public class AuthService(
    DatabaseContext dbContext,
    IPasswordService passwordService,
    IAccessTokenService accessTokenService) : IAuthService
{
    public async Task<SignInResponse> SignInAsync(SignInRequest request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.Include(e => e.Token).FirstOrDefaultAsync(e =>
                e.Login == request.Login.ToLower(),
            cancellationToken
        ) ?? throw new UnauthorizedException("Nieprawidłowy login lub hasło");

        if (!passwordService.VerifyHashedPassword(user.PasswordHash, request.Password))
        {
            throw new UnauthorizedException("Nieprawidłowy login lub hasło");
        }

        var accessToken = accessTokenService.GenerateAccessToken(new AccessTokenDescriptor(
            user.Id.ToString(),
            user.Login,
            user.Role.ToString() 
        ));
        var refreshToken = accessTokenService.GenerateRefreshToken();

        var token = await dbContext.Tokens.FirstOrDefaultAsync(e => e.UserId == user.Id, cancellationToken);
        if (token is null)
        {
            token = new Token
            {
                UserId = user.Id,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };
            await dbContext.Tokens.AddAsync(token, cancellationToken);
        }
        else
        {
            token.RefreshToken = refreshToken;
            token.ExpiresAt = DateTime.UtcNow.AddHours(2);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return new SignInResponse(accessToken, refreshToken);
    }

    public async Task<SignUpResponse> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken)
    {
        if (await dbContext.Users.AnyAsync(e => e.Login == request.Login.ToLower(), cancellationToken))
        {
            throw new ConflictException("Ten login jest już zajęty");
        }

        var user = new User
        {
            Login = request.Login.ToLower(),
            PasswordHash = passwordService.HashPassword(request.Password),
            Role = Role.User
        };

        await dbContext.Users.AddAsync(user, cancellationToken);

        var accessToken = accessTokenService.GenerateAccessToken(new AccessTokenDescriptor(
            user.Id.ToString(),
            user.Login,
            user.Role.ToString()
        ));
        var refreshToken = accessTokenService.GenerateRefreshToken();

        var token = new Token
        {
            UserId = user.Id,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(2)
        };

        await dbContext.Tokens.AddAsync(token, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new SignUpResponse(accessToken, refreshToken);
    }

    public async Task<SignInResponse> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .Include(e => e.Token)
            .Where(e => e.Token!.RefreshToken == refreshToken && e.Token.ExpiresAt >= DateTime.UtcNow)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            throw new UnauthorizedException("Nieważny token odświeżania");
        }
        
        var accessToken = accessTokenService.GenerateAccessToken(new AccessTokenDescriptor(
            user.Id.ToString(),
            user.Login,
            user.Role.ToString()
        ));
        var newRefreshToken = accessTokenService.GenerateRefreshToken();

        var token = await dbContext.Tokens.FirstOrDefaultAsync(e => e.UserId == user.Id, cancellationToken);
        if (token is null)
        {
            token = new Token
            {
                UserId = user.Id,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };
            await dbContext.Tokens.AddAsync(token, cancellationToken);
        }
        else
        {
            token.RefreshToken = newRefreshToken;
            token.ExpiresAt = DateTime.UtcNow.AddHours(2);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return new SignInResponse(accessToken, newRefreshToken);
    }

    public async Task SignOutAsync(string refreshToken, CancellationToken cancellationToken)
    {
        await dbContext.Tokens
            .Where(e => e.RefreshToken == refreshToken)
            .ExecuteDeleteAsync(cancellationToken);
    }
}