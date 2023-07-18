using FullStack.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FullStack.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private FullStackDbContext context;
        public EmployeesController(FullStackDbContext _context)
        {
            context = _context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllEmployee()
        {
            var UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value);
            var employees = await context.Employees.Where(e => e.UserId == UserId).ToListAsync();
            return Ok(employees);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateEmployee(Employee employeeObject)
        {
            int UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value);
            employeeObject.UserId = UserId;
            employeeObject.Id = Guid.NewGuid();
            await context.Employees.AddAsync(employeeObject);
            await context.SaveChangesAsync();

            return Ok(employeeObject);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployee(Employee employeeRequest)
        {
            var employee = await context.Employees.FirstOrDefaultAsync(x => x.Id == employeeRequest.Id);
            if (employee != null)
            {
                employee.Email = employeeRequest.Email;
                employee.Phone = employeeRequest.Phone;
                employee.Salary = employeeRequest.Salary;
                employee.Department = employeeRequest.Department;
                employee.Name = employeeRequest.Name;

                //context.Attach(employee);
                //context.Entry(employee).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }


            return Ok(employee);
        }
        [Authorize]
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetEmployeeById([FromRoute] Guid id)
        {
            var UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value);
            var employee = await context.Employees.Where(e => e.Id == id && e.UserId == UserId).FirstOrDefaultAsync();

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid id)
        {

            var UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value);
            var employee = await context.Employees.Where(e => e.Id == id && e.UserId == UserId).FirstOrDefaultAsync();

            if (employee == null)
            {
                return NotFound();
            }

            context.Employees.Remove(employee);
            await context.SaveChangesAsync();

            return Ok(employee);
        }
    }
}
