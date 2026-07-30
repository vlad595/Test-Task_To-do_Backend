using System;
using Data;
using DTO;
using Models;

namespace Service
{
    public interface ITaskService
    {
        Task<TaskResponseDto> CreateTaskAsync(TaskCreationDto taskDto, Guid userId);
    }

    public class TaskService : ITaskService
    {
        private readonly AppDBContext _context;

        public TaskService(AppDBContext context)
        {
            _context = context;
        }
        public async Task<TaskResponseDto> CreateTaskAsync(TaskCreationDto taskDto, Guid userId)
        {
            var task = new ToDoTask
            {
                Id = Guid.NewGuid(),
                Title = taskDto.Title,
                Description = taskDto.Description,
                Deadline = taskDto.Deadline,
                CategoryId = taskDto.CategoryId,
                UserId = userId,
                IsCompleted = false
            };
            _context.ToDoItems.Add(task);
            await _context.SaveChangesAsync();

            return new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Deadline = task.Deadline,
                IsCompleted = false  
            };
        }
    }
}