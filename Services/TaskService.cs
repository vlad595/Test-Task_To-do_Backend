using System;
using Data;
using DTO;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Service
{
    public interface ITaskService
    {
        Task<TaskResponseDto> CreateTaskAsync(TaskCreationDto taskDto, Guid userId);
        Task<int> DeleteTaskAsync(Guid taskId, Guid userId);
        Task<List<TaskResponseDto>> GetTasksByCategoryAsync(int categoryId, Guid userId);
        Task<TaskResponseDto> GetTaskAsync(Guid taskId, Guid userId);
        Task<List<TaskResponseDto>> GetAllTasks(Guid userId);
        Task<TaskResponseDto> ToggleTaskAsync(Guid taskId, Guid userId);
        Task<TaskResponseDto> UpdateTaskAsync(TaskCreationDto dto, Guid taskId, Guid userId);
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
                Priority = taskDto.PriorityLevel,
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
                PriorityLevel = task.Priority,
                CategoryId = task.CategoryId,
                IsCompleted = false  
            };
        }
        public async  Task<int> DeleteTaskAsync(Guid taskId, Guid userId)
        {
            var task = await _context.ToDoItems.FindAsync(taskId);
            
            if (task == null)
            {
                return 1;
            }
            if (task.UserId != userId)
            {
                return 2;
            }

            _context.ToDoItems.Remove(task);
            await _context.SaveChangesAsync();
            
            return 0;
        }
        public async Task<List<TaskResponseDto>> GetTasksByCategoryAsync(int categoryId, Guid userId)
        {
            return await _context.ToDoItems.Where(t => t.CategoryId == categoryId && t.UserId == userId)
                .Select(t => new TaskResponseDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Deadline = t.Deadline,
                    IsCompleted = t.IsCompleted,
                    PriorityLevel = t.Priority,
                    CategoryId = t.CategoryId
                }).ToListAsync();
        }
        public async Task<TaskResponseDto> GetTaskAsync(Guid taskId, Guid userId)
        {
            var task = await _context.ToDoItems.FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);

            if (task == null)
            {
                return null;
            }

            return new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Deadline = task.Deadline,
                IsCompleted = task.IsCompleted,
                PriorityLevel = task.Priority,
                CategoryId = task.CategoryId
            };
        }
        public async Task<List<TaskResponseDto>> GetAllTasks(Guid userId)
        {
            var tasks = await _context.ToDoItems.Where(task => task.UserId == userId)
                .Select(c => new TaskResponseDto{
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Deadline = c.Deadline,
                    IsCompleted = c.IsCompleted,
                    PriorityLevel = c.Priority,
                    CategoryId = c.CategoryId
                }).ToListAsync();
            if (tasks == null)
            {
                return null;
            }
            return tasks;
        }
        public async Task<TaskResponseDto> ToggleTaskAsync(Guid taskId, Guid userId)
        {
            var task = await _context.ToDoItems.FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);

            if (task == null)
            {
                return null;
            }

            task.IsCompleted = !task.IsCompleted;
            
            await _context.SaveChangesAsync();

            return new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Deadline = task.Deadline,
                IsCompleted = task.IsCompleted,
                PriorityLevel = task.Priority,
                CategoryId = task.CategoryId,
            };
        }
        public async Task<TaskResponseDto> UpdateTaskAsync(TaskCreationDto dto, Guid taskId, Guid userId)
        {
            var task = await _context.ToDoItems.FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);

            if (task == null)
            {
                return null;
            }

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.Deadline = dto.Deadline;
            task.CategoryId = dto.CategoryId; 

            await _context.SaveChangesAsync();

            return new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Deadline = task.Deadline,
                IsCompleted = task.IsCompleted,
                CategoryId = task.CategoryId,
                PriorityLevel = task.Priority
            };
        }
    }
}