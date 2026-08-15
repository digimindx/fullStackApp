using CORE.Data;
using CORE.Entities.HR;
using CORE.Interfaces.HR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CORE.Repositories.HR;

public class EmployeeAccountRepository : IEmployeeAccountRepository
{
    private readonly AppDbContext _context;

    public EmployeeAccountRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<EmployeeAccount?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.EmployeeAccounts
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.AccountID == id, cancellationToken);
    }

    public async Task<IEnumerable<EmployeeAccount>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.EmployeeAccounts
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<EmployeeAccount>> FindAsync(Expression<Func<EmployeeAccount, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.EmployeeAccounts
            .AsNoTracking()
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<EmployeeAccount> AddAsync(EmployeeAccount entity, CancellationToken cancellationToken = default)
    {
        await _context.EmployeeAccounts.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(EmployeeAccount entity, CancellationToken cancellationToken = default)
    {
        _context.EmployeeAccounts.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var account = await _context.EmployeeAccounts.FindAsync([id], cancellationToken);
        if (account != null)
        {
            _context.EmployeeAccounts.Remove(account);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.EmployeeAccounts.AnyAsync(a => a.AccountID == id, cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.EmployeeAccounts.CountAsync(cancellationToken);
    }

    // ─── Domain-specific methods ─────────────────────────────────────

    public async Task<EmployeeAccount?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _context.EmployeeAccounts
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Username == username, cancellationToken);
    }

    public async Task<EmployeeAccount?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.EmployeeAccounts
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Email == email, cancellationToken);
    }

    public async Task<bool> IsUsernameAvailableAsync(string username, CancellationToken cancellationToken = default)
    {
        return !await _context.EmployeeAccounts.AnyAsync(a => a.Username == username, cancellationToken);
    }

    public async Task<bool> IsEmailAvailableAsync(string email, CancellationToken cancellationToken = default)
    {
        return !await _context.EmployeeAccounts.AnyAsync(a => a.Email == email, cancellationToken);
    }

    public async Task UpdateLoginAttemptAsync(int accountID, int failedAttempts, CancellationToken cancellationToken = default)
    {
        var account = await _context.EmployeeAccounts
            .FirstOrDefaultAsync(a => a.AccountID == accountID, cancellationToken);

        if (account != null)
        {
            account.FailedLoginAttempts = failedAttempts;
            account.LastLoginDate = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task UnlockAccountAsync(int accountID, CancellationToken cancellationToken = default)
    {
        var account = await _context.EmployeeAccounts
            .FirstOrDefaultAsync(a => a.AccountID == accountID, cancellationToken);

        if (account != null)
        {
            account.IsLocked = false;
            account.LockedUntil = null;
            account.FailedLoginAttempts = 0;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task UpdateRefreshTokenAsync(int accountID, string? refreshToken, DateTime? expiryTime, CancellationToken cancellationToken = default)
    {
        var account = await _context.EmployeeAccounts
            .FirstOrDefaultAsync(a => a.AccountID == accountID, cancellationToken);

        if (account != null)
        {
            account.RefreshToken = refreshToken;
            account.RefreshTokenExpiryTime = expiryTime;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
