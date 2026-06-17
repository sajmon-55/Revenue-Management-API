namespace Application.Clients.DTOs;

public record AddCompanyClientRequest(
    string Name, 
    string Email, 
    string Phone, 
    string Address, 
    string Krs
);