using AdressBook.Domain.ViewModels.Employee;
using AdressBook.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdressBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> AddAsync([FromForm] CreateEmployeeDto employeeDto)
        {
            var response = await _employeeService.CreateAsync(employeeDto);
            return (response.Succeeded) ? Ok(response) : BadRequest(response);
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(string? searchtext)
        {
            var response = await _employeeService.GetAllAsync(searchtext);
            return (response.Succeeded) ? Ok(response) : BadRequest(response);
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int Id)
        {
            var response = await _employeeService.GetById(Id);
            return (response.Succeeded) ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("SoftDelete")]
        public async Task<IActionResult> SoftDelete(int Id)
        {
            var response = await _employeeService.SoftDelete(Id);
            return (response.Succeeded) ? Ok(response) : BadRequest(response);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromForm] UpdateEmployeeDto employeeDto)
        {
            var response = await _employeeService.UpdateAsync(employeeDto);
            return (response.Succeeded) ? Ok(response) : BadRequest(response);
        }
    }
}