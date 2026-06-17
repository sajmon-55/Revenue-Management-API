using System.ComponentModel.DataAnnotations;

namespace Application.Softwares.DTOs;

public record CreateSoftwareRequest(
    [Required, MaxLength(100)]
    string Name,
    [Required, MaxLength(300)]
    string Description,
    [Required, MaxLength(20)]
    string CurrentVersion,
    [Required, MaxLength(50)]
    string Category
);