using AdressBook.Domain.ViewModels.Common;
using AdressBook.Services.IServices;
using AdressBook.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdressBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(string? searchtext)
        {
            var response = await _departmentService.GetAllAsync(searchtext);
            return (response.Succeeded) ? Ok(response) : BadRequest(response);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> AddAsync(CreateModelDto modelDto)
        {
            var response = await _departmentService.CreateAsync(modelDto);
            return (response.Succeeded) ? Ok(response) : BadRequest(response);
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int Id)
        {
            var response = await _departmentService.GetById(Id);
            return (response.Succeeded) ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("SoftDelete")]
        public async Task<IActionResult> SoftDelete(int Id)
        {
            var response = await _departmentService.SoftDelete(Id);
            return (response.Succeeded) ? Ok(response) : BadRequest(response);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update(UpdateModelDto modelDto)
        {
            var response = await _departmentService.UpdateAsync(modelDto);
            return (response.Succeeded) ? Ok(response) : BadRequest(response);
        }
    }
}