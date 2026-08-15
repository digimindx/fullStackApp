using CORE.Data;
using CORE.Entities.HR;
using CORE.Interfaces.HR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CORE.Repositories.HR;

public class EmployeeBankAccountRepository : IEmployeeBankAccountRepository
{
    private readonly AppDbContext _context;

    public EmployeeBankAccountRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<EmployeeBankAccount?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.EmployeeBankAccounts
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.BankAccountID == id, cancellationToken);
    }

    public async Task<IEnumerable<EmployeeBankAccount>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.EmployeeBankAccounts
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<EmployeeBankAccount>> FindAsync(Expression<Func<EmployeeBankAccount, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.EmployeeBankAccounts
            .AsNoTracking()
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<EmployeeBankAccount> AddAsync(EmployeeBankAccount entity, CancellationToken cancellationToken = default)
    {
        await _context.EmployeeBankAccounts.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(EmployeeBankAccount entity, CancellationToken cancellationToken = default)
    {
        _context.EmployeeBankAccounts.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var account = await _context.EmployeeBankAccounts.FindAsync([id], cancellationToken);
        if (account != null)
        {
            _context.EmployeeBankAccounts.Remove(account);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.EmployeeBankAccounts.AnyAsync(b => b.BankAccountID == id, cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.EmployeeBankAccounts.CountAsync(cancellationToken);
    }

    // ─── Domain-specific methods ─────────────────────────────────────

    public async Task<IEnumerable<EmployeeBankAccount>> GetByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken = default)
    {
        return await _context.EmployeeBankAccounts
            .AsNoTracking()
            .Where(b => b.EmployeeID == employeeId)
            .ToListAsync(cancellationToken);
    }

    public async Task<EmployeeBankAccount?> GetActiveByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken = default)
    {
        return await _context.EmployeeBankAccounts
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.EmployeeID == employeeId && b.IsActive, cancellationToken);
    }
}
