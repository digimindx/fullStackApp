using CORE.Entities.HR;

namespace CORE.Interfaces.HR;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<Employee?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<Employee?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<Employee>> GetActiveEmployeesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Employee>> SearchAsync(string query, CancellationToken cancellationToken = default);
}
