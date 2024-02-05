using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using WebAPIREST.Models;

namespace WebAPIREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly IEmployeeRepository _repository;

        public EmployeeController(IEmployeeRepository repository)
        {
            _repository = repository;

        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>?>> GetEmployees()
        {
            if (await _repository.GetAllEmployee() == null)
            {
                return NotFound();
            }

            return await _repository.GetAllEmployee();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetById_ActionResultOfT(int id)
        {
            var employee = await _repository.GetEmployee(id);
            return employee == null ? NotFound() : employee;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }
            try
            {
                await _repository.Update(id, employee);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_repository.GetEmployee(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            await _repository.Add(employee);
            return CreatedAtAction("PostEmployee", new { id = employee.Id }, employee);
        }
        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (_repository.GetAllEmployee() == null)
            {
                return NotFound();
            }
            var employee = _repository.Delete(id);
            if (employee == null)
            {
                return NotFound();
            }
            await _repository.Delete(employee.Id);
            return NoContent();
        }


    }
}
