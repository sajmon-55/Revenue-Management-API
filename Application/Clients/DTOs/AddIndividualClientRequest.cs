namespace Application.Clients.DTOs;

public record AddIndividualClientRequest(
    string FirstName, 
    string LastName, 
    string Email, 
    string Phone, 
    string Address, 
    string Pesel
);