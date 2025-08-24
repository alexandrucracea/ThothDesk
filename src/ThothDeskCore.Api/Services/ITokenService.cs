using ThothDeskCore.Infrastructure;

namespace ThothDeskCore.Api.Services;

public interface ITokenService
{
    string GenerateToken(ApplicationUser user);
}

