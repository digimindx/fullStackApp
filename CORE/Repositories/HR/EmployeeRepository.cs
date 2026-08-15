using CORE.Data;
using CORE.Entities.HR;
using CORE.Interfaces.HR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CORE.Repositories.HR;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _context;

    public EmployeeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Employee?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.EmployeeID == id, cancellationToken);
    }

    public async Task<IEnumerable<Employee>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Employees
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Employee>> FindAsync(Expression<Func<Employee, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Employees
            .AsNoTracking()
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Employee> AddAsync(Employee entity, CancellationToken cancellationToken = default)
    {
        await _context.Employees.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(Employee entity, CancellationToken cancellationToken = default)
    {
        _context.Employees.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var employee = await _context.Employees.FindAsync([id], cancellationToken);
        if (employee != null)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Employees.AnyAsync(e => e.EmployeeID == id, cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Employees.CountAsync(cancellationToken);
    }

    // ─── Domain-specific methods ─────────────────────────────────────

    public async Task<Employee?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _context.Employees
            .Include(e => e.EmployeeAccount)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.EmployeeAccount != null && e.EmployeeAccount.Username == username, cancellationToken);
    }

    public async Task<Employee?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Employees
            .Include(e => e.EmployeeAccount)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.EmployeeAccount != null && e.EmployeeAccount.Email == email, cancellationToken);
    }

    public async Task<IEnumerable<Employee>> GetActiveEmployeesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Employees
            .AsNoTracking()
            .Where(e => e.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Employee>> SearchAsync(string query, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return await GetAllAsync(cancellationToken);

        return await _context.Employees
            .AsNoTracking()
            .Where(e => e.FullName.Contains(query) ||
                        e.LastName.Contains(query) ||
                        (e.EmployeeNumber != null && e.EmployeeNumber.ToString().Contains(query)))
            .ToListAsync(cancellationToken);
    }
}
