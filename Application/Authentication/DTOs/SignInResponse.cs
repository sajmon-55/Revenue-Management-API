namespace Application.Authentication.DTOs;

public record SignInResponse(
    string AccessToken,
    string RefreshToken
);