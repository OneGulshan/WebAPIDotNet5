using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationAPI.Data;

namespace WebApplicationAPI.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> AddEmployees(Employee employee)
        {
            var result = await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return result.Entity;//Entity <- This Entity is model our saved result tracked by this Entity and auto get our data for displaing on browser with the help of this auto tracking Entity.
        }

        public async Task<Employee> DeleteEmployee(int Id)
        {
            var result = await _context.Employees.Where(x => x.Id == Id).FirstOrDefaultAsync();
            if (result != null)
            {
                _context.Employees.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();             
        }

        public async Task<Employee> GetEmployee(int Id)
        {
            return await _context.Employees.Where(x => x.Id == Id).FirstOrDefaultAsync();             
        }

        public async Task<Employee> UpdateEmployees(Employee employee)
        {
            var result = await _context.Employees.FirstOrDefaultAsync(x => x.Id == employee.Id);
            if (result != null)
            {
                result.Name = employee.Name;
                result.City = employee.City;
                await _context.SaveChangesAsync();
                return result;
            }            
            return null;
        }

        public async Task<IEnumerable<Employee>> Search(string name)
        {
            IQueryable<Employee> query = _context.Employees;
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(a => a.Name.Contains(name));//Contains for deep searching
            }
            return await query.ToListAsync();
        }
    }
}
