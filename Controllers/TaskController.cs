using System;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        private Guid GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Guid.TryParse(userIdClaim, out Guid userId);
            return userId; 
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskCreationDto taskDto)
        {
            var userId = GetUserId();
            if (userId == Guid.Empty) return Unauthorized();

            var result = await _taskService.CreateTaskAsync(taskDto, userId);
            
            return CreatedAtAction(nameof(GetTask), new { taskId = result.Id }, result);
        }

        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTask(Guid taskId)
        {
            var userId = GetUserId();
            if (userId == Guid.Empty) return Unauthorized();

            var task = await _taskService.GetTaskAsync(taskId, userId);
            
            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetTasksByCategory(int categoryId)
        {
            var userId = GetUserId();
            if (userId == Guid.Empty) return Unauthorized();

            var tasks = await _taskService.GetTasksByCategoryAsync(categoryId, userId);
            return Ok(tasks);
        }

        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateTask(Guid taskId, [FromBody] TaskCreationDto dto)
        {
            var userId = GetUserId();
            if (userId == Guid.Empty) return Unauthorized();

            var updatedTask = await _taskService.UpdateTaskAsync(dto, taskId, userId);
            
            if (updatedTask == null)
            {
                return NotFound();
            }

            return Ok(updatedTask);
        }

        [HttpPatch("{taskId}/complete")]
        public async Task<IActionResult> CompleteTask(Guid taskId)
        {
            var userId = GetUserId();
            if (userId == Guid.Empty) return Unauthorized();

            var completedTask = await _taskService.CompleteTaskAsync(taskId, userId);
            
            if (completedTask == null)
            {
                return NotFound();
            }

            return Ok(completedTask);
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(Guid taskId)
        {
            var userId = GetUserId();
            if (userId == Guid.Empty) return Unauthorized();

            var result = await _taskService.DeleteTaskAsync(taskId, userId);

            return result switch
            {
                0 => NoContent(),
                1 => NotFound(),
                2 => Forbid(),
                _ => StatusCode(500)
            };
        }
    }
}