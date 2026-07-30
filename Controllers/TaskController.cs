using System;
using System.Runtime.CompilerServices;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskCreationDto taskDto)
        {
            var mockUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");

            var result = await _taskService.CreateTaskAsync(taskDto, mockUserId);
            return CreatedAtAction(nameof(CreateTask), new {id = result.Id}, result);
        }
    }
}