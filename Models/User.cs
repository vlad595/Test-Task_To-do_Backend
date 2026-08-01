using System;

namespace Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public ICollection<ToDoTask> Tasks { get; set; } = new List<ToDoTask>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}