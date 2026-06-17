using Application.Exceptions;
using Application.Softwares.DTOs;
using Domain.Entities.Softwares;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Softwares;

public class SoftwareService(DatabaseContext dbContext) : ISoftwareService
{
    public async Task AddSoftwareAsync(CreateSoftwareRequest request, CancellationToken cancellationToken)
    {
        var softwareExists = await dbContext.Softwares.AnyAsync(s => s.Name == request.Name, cancellationToken);
        if (softwareExists)
        {
            throw new ConflictException("Software already exists");
        }

        var software = new Software
        {
            Name = request.Name,
            Description = request.Description,
            CurrentVersion = request.CurrentVersion,
            Category = request.Category,
        };
        
        await dbContext.Softwares.AddAsync(software, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}