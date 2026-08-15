using CORE.Entities.HR;

namespace CORE.Interfaces.HR;

public interface IEmployeeBankAccountRepository : IRepository<EmployeeBankAccount>
{
    Task<IEnumerable<EmployeeBankAccount>> GetByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken = default);
    Task<EmployeeBankAccount?> GetActiveByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken = default);
}
