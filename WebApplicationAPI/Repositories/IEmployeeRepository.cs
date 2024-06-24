using DataAccessLayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplicationAPI.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> Search(string name);
        Task<IEnumerable<Employee>> GetEmployees();
        Task<Employee> GetEmployee(int Id);
        Task<Employee> AddEmployees(Employee employee);
        Task<Employee> UpdateEmployees(Employee employee);
        Task<Employee> DeleteEmployee(int Id);
    }
}
