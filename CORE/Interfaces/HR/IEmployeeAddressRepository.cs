using CORE.Entities.HR;

namespace CORE.Interfaces.HR;

public interface IEmployeeAddressRepository : IRepository<EmployeeAddress>
{
    Task<IEnumerable<EmployeeAddress>> GetByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken = default);
    Task<EmployeeAddress?> GetCurrentAddressAsync(int employeeId, CancellationToken cancellationToken = default);
    Task<EmployeeAddress?> GetPermanentAddressAsync(int employeeId, CancellationToken cancellationToken = default);
    Task SetPrimaryAddressAsync(int addressId, CancellationToken cancellationToken = default);
    Task<IEnumerable<EmployeeAddress>> GetActiveAddressesAsync(int employeeId, CancellationToken cancellationToken = default);
}
