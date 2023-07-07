using FullStack.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FullStack.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private FullStackDbContext context;
        public EmployeesController(FullStackDbContext _context) { 
            context = _context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllEmployee()
        {
            var employees = await context.Employees.ToListAsync();
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(Employee employeeObject)
        {
            employeeObject.Id = Guid.NewGuid();
            await context.Employees.AddAsync(employeeObject);
            await context.SaveChangesAsync();

            return Ok(employeeObject);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployee(Employee employeeRequest)
        {
            var employee = await context.Employees.FirstOrDefaultAsync(x=> x.Id== employeeRequest.Id);
            if(employee != null)
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
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetEmployeeById([FromRoute] Guid id)
        {
            var employee = await context.Employees.FindAsync(id);

            if(employee == null )
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid id)
        {
            var employee = await context.Employees.FindAsync(id);

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
