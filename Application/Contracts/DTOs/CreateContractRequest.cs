using System.ComponentModel.DataAnnotations;

namespace Application.Contracts.DTOs;

public record CreateContractRequest(
    [Required]
    int ClientId,
    [Required]
    int SoftwareId,
    [Required, MaxLength(20)]
    string SoftwareVersion,
    [Required]
    DateOnly StartDate,
    [Required]
    DateOnly EndDate,
    [Required]
    decimal Price,
    [Required]
    int YearsOfSupport
);