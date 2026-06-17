using Application.Softwares.DTOs;

namespace Application.Softwares;

public interface ISoftwareService
{
    Task AddSoftwareAsync(CreateSoftwareRequest request, CancellationToken cancellationToken);
}