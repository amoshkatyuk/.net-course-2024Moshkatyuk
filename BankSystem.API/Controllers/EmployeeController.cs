using AutoMapper;
using BankSystem.App.Dto;
using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace BankSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        [HttpGet("by guid")]
        public async Task<IActionResult> GetEmployee([FromQuery] Guid employeeId, CancellationToken cancellationToken)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(employeeId, cancellationToken);

            if (employee == null)
            {
                return NotFound();
            }

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            return Ok(employeeDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeDto employeeDto, CancellationToken cancellationToken)
        {
            var employee = _mapper.Map<Employee>(employeeDto);

            await _employeeService.AddEmployeeAsync(employee, cancellationToken);

            return Ok(employee);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployee(Guid employeeId, [FromBody] EmployeeDto employeeDto, CancellationToken cancellationToken)
        {
            if (employeeDto == null) 
            {
                return BadRequest("Данные для обновления сущности не были предоставлены");
            }

            var existingEmployee = await _employeeService.GetEmployeeByIdAsync(employeeId, cancellationToken);

            _mapper.Map(employeeDto, existingEmployee);

            await _employeeService.UpdateEmployeeAsync(existingEmployee, cancellationToken);

            return Ok(existingEmployee);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEmployee([FromQuery] Guid employeeId, CancellationToken cancellationToken)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(employeeId, cancellationToken);

            if (employee == null)
            {
                return NotFound();
            }

            await _employeeService.DeleteEmployeeAsync(employeeId, cancellationToken);

            return Ok();
        }

        [HttpGet("by filter")]
        public async Task<ActionResult<List<EmployeeDto>>> SearchClients(
            CancellationToken cancellationToken,
            [FromQuery] string name = null,
            [FromQuery] string surname = null,
            [FromQuery] string passportData = null,
            [FromQuery] DateTimeOffset? birthDate = null,
            [FromQuery] decimal? salary = null,
            [FromQuery] string position = null,
            [FromQuery] string contract = null)
        {
            Expression<Func<Employee, bool>> filter = e =>
            (string.IsNullOrEmpty(name) || e.Name.Contains(name)) &&
            (string.IsNullOrEmpty(surname) || e.Surname.Contains(surname)) &&
            (string.IsNullOrEmpty(passportData) || e.PassportData.Contains(passportData)) &&
            (!birthDate.HasValue || e.BirthDate == birthDate) &&
            (!salary.HasValue || e.Salary == salary) &&
            (string.IsNullOrEmpty(position) || e.Position.Contains(position)) &&
            (string.IsNullOrEmpty(contract) || e.Contract.Contains(contract));

            var filteredEmployees = await _employeeService.FilterEmployeesAsync(filter, cancellationToken);

            var filteredEmployeeDtos = _mapper.Map<List<EmployeeDto>>(filteredEmployees);

            return Ok(filteredEmployeeDtos);
        }
    }
}
