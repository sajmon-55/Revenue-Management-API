using System.ComponentModel.DataAnnotations;

namespace Application.Authentication.DTOs;

public class SignUpRequest
{
    [Required, MaxLength(50)] 
    public string Login { get; set; } = string.Empty;
    
    [Required, MaxLength(255)] 
    public string Password { get; set; } = string.Empty;
    
    [Required, MaxLength(255), Compare(nameof(Password))] 
    public string RepeatPassword { get; set; } = string.Empty;
}