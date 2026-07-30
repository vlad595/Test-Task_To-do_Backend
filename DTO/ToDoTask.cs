using System;
using Models;

namespace DTO
{
    public class TaskCreationDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Deadline { get; set; }
        public PriorityLevel PriorityLevel { get; set; }
        public int? CategoryId { get; set; }
    }
    public class TaskResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? Deadline { get; set; }
        public bool IsCompleted { get; set; }
    }
}