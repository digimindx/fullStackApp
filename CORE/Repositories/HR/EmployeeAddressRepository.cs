using CORE.Data;
using CORE.Entities.HR;
using CORE.Interfaces.HR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CORE.Repositories.HR;

public class EmployeeAddressRepository : IEmployeeAddressRepository
{
    private readonly AppDbContext _context;

    public EmployeeAddressRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<EmployeeAddress?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.EmployeeAddresses
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.AddressID == id, cancellationToken);
    }

    public async Task<IEnumerable<EmployeeAddress>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.EmployeeAddresses
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<EmployeeAddress>> FindAsync(Expression<Func<EmployeeAddress, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.EmployeeAddresses
            .AsNoTracking()
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<EmployeeAddress> AddAsync(EmployeeAddress entity, CancellationToken cancellationToken = default)
    {
        await _context.EmployeeAddresses.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(EmployeeAddress entity, CancellationToken cancellationToken = default)
    {
        _context.EmployeeAddresses.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var address = await _context.EmployeeAddresses.FindAsync([id], cancellationToken);
        if (address != null)
        {
            _context.EmployeeAddresses.Remove(address);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.EmployeeAddresses.AnyAsync(a => a.AddressID == id, cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.EmployeeAddresses.CountAsync(cancellationToken);
    }

    // ─── Domain-specific methods ─────────────────────────────────────

    public async Task<IEnumerable<EmployeeAddress>> GetByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken = default)
    {
        return await _context.EmployeeAddresses
            .AsNoTracking()
            .Where(a => a.EmployeeID == employeeId)
            .ToListAsync(cancellationToken);
    }

    public async Task<EmployeeAddress?> GetCurrentAddressAsync(int employeeId, CancellationToken cancellationToken = default)
    {
        return await _context.EmployeeAddresses
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.EmployeeID == employeeId && a.AddressType == 'C', cancellationToken);
    }

    public async Task<EmployeeAddress?> GetPermanentAddressAsync(int employeeId, CancellationToken cancellationToken = default)
    {
        return await _context.EmployeeAddresses
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.EmployeeID == employeeId && a.AddressType == 'P', cancellationToken);
    }

    public async Task SetPrimaryAddressAsync(int addressId, CancellationToken cancellationToken = default)
    {
        var address = await _context.EmployeeAddresses
            .FirstOrDefaultAsync(a => a.AddressID == addressId, cancellationToken);

        if (address != null)
        {
            // Unset all other primary addresses for this employee
            var employeeId = address.EmployeeID;
            var otherAddresses = await _context.EmployeeAddresses
                .Where(a => a.EmployeeID == employeeId && a.IsPrimary)
                .ToListAsync(cancellationToken);

            foreach (var other in otherAddresses)
            {
                other.IsPrimary = false;
            }

            address.IsPrimary = true;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<IEnumerable<EmployeeAddress>> GetActiveAddressesAsync(int employeeId, CancellationToken cancellationToken = default)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        return await _context.EmployeeAddresses
            .AsNoTracking()
            .Where(a => a.EmployeeID == employeeId &&
                        a.ValidFrom <= today &&
                        (a.ValidTo == null || a.ValidTo >= today))
            .ToListAsync(cancellationToken);
    }
}
