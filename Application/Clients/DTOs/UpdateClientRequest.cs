namespace Application.Clients.DTOs;

public record UpdateClientRequest(
    string Email,
    string Phone,
    string Address,
    string? FirstName, 
    string? LastName,
    string? CompanyName
);