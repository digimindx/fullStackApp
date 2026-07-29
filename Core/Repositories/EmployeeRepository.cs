using Core.Data;
using Core.Entities;
using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories
{
    public class EmployeeRepository : IEmployee
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<Employee?> GetByUsernameOrEmailAsync(string usernameOrEmail)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.Username == usernameOrEmail || e.Email == usernameOrEmail);
        }

        public async Task<bool> IsUsernameOrEmailExistsAsync(string username, string email)
        {
            return await _context.Employees
                .AnyAsync(e => e.Username == username || e.Email == email);
        }

        public async Task<bool> RegisterAsync(RegisterModel model)
        {
            // 1. Check if the user already exists
            if (await IsUsernameOrEmailExistsAsync(model.Username, model.Email))
            {
                return false;
            }

            // 2. Generate a unique ID (since ID is not identity in the SQL schema provided)
            int newId = 1;
            if (await _context.Employees.AnyAsync())
            {
                newId = await _context.Employees.MaxAsync(e => e.ID) + 1;
            }

            // 3. Map RegisterModel to Employee Entity
            var newEmployee = new Employee
            {
               // ID = newId,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Email = model.Email,
                Username = model.Username,
                // NOTE: Best practice is to hash this password before storing it!
                Password = model.Password
            };

            await _context.Employees.AddAsync(newEmployee);
            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0;
        }

        public async Task<Employee?> AuthenticateAsync(LoginModel model)
        {
            var employee = await GetByUsernameOrEmailAsync(model.Username);

            if (employee == null)
            {
                return null;
            }

            // NOTE: Replace this direct comparison with password hash verification if hashing is used during registration
            if (employee.Password == model.Password)
            {
                return employee;
            }

            return null;
        }

        public async Task<bool> UpdateAsync(Employee employee)
        {
            // Find existing entity by ID
            var existing = await _context.Employees.FindAsync(employee.ID);
            if (existing == null)
            {
                return false;
            }

            // Update fields - overwrite with provided values
            existing.FirstName = employee.FirstName;
            existing.MiddleName = employee.MiddleName;
            existing.LastName = employee.LastName;
            existing.Email = employee.Email;
            existing.Username = employee.Username;
            existing.Password = employee.Password;

            _context.Employees.Update(existing);
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }
    }
}