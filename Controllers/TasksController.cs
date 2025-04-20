using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using TaskManager_api.Models;
using TaskManager_api.Services;

namespace TaskManager_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly TaskService _taskService;

        public TasksController(TaskService taskService)
        {
            _taskService = taskService;
        }

        private (string username, string role) GetUserDetails()
        {
            var username = User.Identity?.Name;
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            return (username, role);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll()
        {
            var (username, role) = GetUserDetails();

            if (username == null || role == null)
                return Unauthorized();

            var tasks = await _taskService.GetTasksByUserAsync(username, role);
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetById(int id)
        {
            var (username, role) = GetUserDetails();

            if (username == null || role == null)
                return Unauthorized();

            var task = await _taskService.GetByIdAsync(id);
            if (task == null)
                return NotFound();

            if (role != "Admin" && task.AssignedTo != username)
                return Forbid();

            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> Create(TaskItem task)
        {
            var (username, role) = GetUserDetails();

            if (username == null || role == null)
                return Unauthorized();

            if (role == "User")
            {
                if (string.IsNullOrEmpty(task.AssignedTo))
                {
                    task.AssignedTo = username;
                }
                else if (task.AssignedTo != username)
                {
                    return Forbid();  
                }
            }

            if (role == "Admin")
            {
                
                if (string.IsNullOrEmpty(task.AssignedTo))
                {
                    return BadRequest("Admin must assign the task to a user.");
                }
            }

            var created = await _taskService.CreateAsync(task);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TaskItem task)
        {
            var (username, role) = GetUserDetails();

            if (username == null || role == null)
                return Unauthorized();

            var existing = await _taskService.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            if (role != "Admin" && existing.AssignedTo != username)
                return Forbid();

            await _taskService.UpdateAsync(id, task);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var (username, role) = GetUserDetails();

            if (username == null || role == null)
                return Unauthorized();

            var existing = await _taskService.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            if (role != "Admin" && existing.AssignedTo != username)
                return Forbid();

            await _taskService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/assign")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignToUser(int id, [FromBody] AssignDto dto)
        {
            var existing = await _taskService.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _taskService.AssignTaskAsync(id, dto.AssignedTo);
            return NoContent();
        }

        public class AssignDto
        {
            [Required]
            public string AssignedTo { get; set; }
        }
    }
}
