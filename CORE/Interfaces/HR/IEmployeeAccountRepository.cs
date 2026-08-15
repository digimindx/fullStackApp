using CORE.Entities.HR;

namespace CORE.Interfaces.HR;

public interface IEmployeeAccountRepository : IRepository<EmployeeAccount>
{
    Task<EmployeeAccount?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<EmployeeAccount?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> IsUsernameAvailableAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> IsEmailAvailableAsync(string email, CancellationToken cancellationToken = default);
    Task UpdateLoginAttemptAsync(int accountID, int failedAttempts, CancellationToken cancellationToken = default);
    Task UnlockAccountAsync(int accountID, CancellationToken cancellationToken = default);
    Task UpdateRefreshTokenAsync(int accountID, string? refreshToken, DateTime? expiryTime, CancellationToken cancellationToken = default);
}
