namespace Application.Authentication.DTOs;

public record SignUpResponse(
    string AccessToken,
    string RefreshToken
);