using System;

namespace Models
{
    public class ToDoTask
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? Deadline { get; set; }
        public bool IsCompleted { get; set; } = false;
        public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;

        public Guid UserId { get; set; }
        public User? User { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
    }

    public enum PriorityLevel
    {
        Low = 0,
        Medium = 1,
        High = 2
    }   
}