using System.ComponentModel.DataAnnotations;

namespace Application.Authentication.DTOs;

public record SignInRequest(
    [Required, MaxLength(50)] string Login,
    [Required, MaxLength(255)] string Password
);