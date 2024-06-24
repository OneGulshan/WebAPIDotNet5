using DataAccessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationAPI.Repositories;

namespace WebApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        [HttpGet]
        public async Task<ActionResult> GetEmployees()
        {
            try
            {
                return Ok(await _employeeRepository.GetEmployees());
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error in Retriving Data from Database");
            }            
        }

        [HttpGet("{id:int}")]//Multimatch //{} <- curly braces, "" <- double cotetion //id:int <- id with data type for saving from confliction
        public async Task<ActionResult<Employee>> GetEmployee(int Id)//<Employee> <- implicitly conversion problem use explicity conversion
        {
            try
            {
                var result = await _employeeRepository.GetEmployee(Id);
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return result;
                }
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error in Retriving Data from Database");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest();
                }
                var CreateEmployee = await _employeeRepository.AddEmployees(employee);
                return CreatedAtAction(nameof(GetEmployee), new { id = CreateEmployee.Id }, CreateEmployee);//CreateEmployee <- Created Employee ki dezired location janne ke liye //CreateEmployee <-Updated Id bhejni hoti hai jo get request ko call kar lega
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error in Retrieving Data from Database");

            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int Id, Employee employee)
        {
            try
            {
                if (Id != employee.Id)
                {
                    return BadRequest("Id Mismatch");
                }
                var employeeUpdate = await _employeeRepository.GetEmployee(Id);//employeeUpdate used for only showing not Found error
                if (employeeUpdate == null)
                {
                    return NotFound($"Employee Id={Id} not Found");
                }
                return await _employeeRepository.UpdateEmployees(employee);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error in Retrieving Data from Database");
            }
        }

        [HttpDelete("{id:int}")]//Route(URL) me id jud jaegi or match ho jaegi
        public async Task<ActionResult<Employee>> DeleteEmploye(int Id)
        {
            try
            {
                var employeeDelete = await _employeeRepository.GetEmployee(Id);
                if (employeeDelete == null)
                {
                    return NotFound($"Employee Id={Id} not Found");
                }
                return await _employeeRepository.DeleteEmployee(Id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error in Retriving Data from Database");
            }
        }

        [HttpGet("{search}")]
        public async Task<ActionResult<IEnumerable<Employee>>> Search(string name)//is name parameter me value dene ke liye query string ka use hoga -> https://localhost:44352/api/employee/Search?name=Gulshan
        {
            try
            {
                var result = await _employeeRepository.Search(name);
                if (result.Any())
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error in Retriving Data from Database");
            }            
        }
    }
}
